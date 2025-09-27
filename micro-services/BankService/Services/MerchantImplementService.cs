////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using TinkoffPaymentClientApi.ResponseEntity;
using TinkoffPaymentClientApi.Commands;
using TinkoffPaymentClientApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TinkoffPaymentClientApi;
using Newtonsoft.Json.Linq;
using SharedLib;
using DbcLib;

namespace BankService;

/// <summary>
/// MerchantImplementService
/// </summary>
public partial class MerchantImplementService(IOptions<TBankSettings> settings, IIdentityTransmission identityRepo, ILogger<MerchantImplementService> loggerRepo, IDbContextFactory<BankContext> bankDbFactory)
    : IMerchantService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel>> BindCustomerTBankAsync(BindCustomerTBankRequestModel req, CancellationToken token = default)
    {
        TinkoffPaymentClient clientApi = new(settings.Value.TerminalKey, settings.Value.Password);
        TResponseModel<UserInfoModel> res = new();
        TResponseModel<UserInfoModel[]> userDb = await identityRepo.GetUsersIdentityAsync([req.UserId], token);

        if (!userDb.Success())
        {
            res.AddRangeMessages(userDb.Messages);
            return res;
        }

        if (userDb.Response is null || userDb.Response.Length == 0)
        {
            res.AddError("User not found");
            return res;
        }

        res.Response = userDb.Response[0];

        try
        {
            await clientApi.GetCustomerAsync(new GetCustomer(req.UserId), token);
        }
        catch (Exception e)
        {
            loggerRepo.LogError(e, nameof(clientApi.GetCustomerAsync));
            res.AddWarning($"Customer by user - Created!");
            await clientApi.AddCustomerAsync(new AddCustomer(req.UserId) { Email = res.Response.Email, Phone = res.Response.PhoneNumber }, token);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentInitTBankResultModelDB>> InitPaymentMerchantTBankAsync(InitMerchantTBankRequestModel req, CancellationToken token = default)
    {
        TResponseModel<PaymentInitTBankResultModelDB> res = new() { Response = req.GetDB() };
        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        await ctx.PaymentsInitResultsTBank.AddAsync(res.Response, token);
        await ctx.SaveChangesAsync(token);

        TResponseModel<UserInfoModel[]> userCreator = await identityRepo.GetUsersIdentityAsync([req.UserId], token);
        if (!userCreator.Success())
        {
            res.AddRangeMessages(userCreator.Messages);
            return res;
        }

        if (userCreator.Response is null || userCreator.Response.Length == 0)
        {
            res.AddError($"user #{req.UserId} not found");
            loggerRepo.LogError($"user #{req.UserId} not found");
            return res;
        }

        TinkoffPaymentClient clientApi = new(settings.Value.TerminalKey, settings.Value.Password);
        Receipt rec = req.Receipt.GetTBankReceipt();

        Init _iReq = new(req.OrderId, req.Amount, req.IsRecurrent, req.UserId)
        {
            Receipt = req.Receipt.GetTBankReceipt(),

            Language = req.Language?.Convert(),
            PayType = req.PayType?.Convert(),

            Description = req.Description,
            FailURL = req.FailURL,
            Data = req.Data,
            IP = req.IP,
            NotificationURL = req.NotificationURL,
            SuccessURL = req.SuccessURL,
            RedirectDueDate = req.RedirectDueDate,
        };

        PaymentResponse resultPayment;
        IQueryable<PaymentInitTBankResultModelDB> q = ctx.PaymentsInitResultsTBank.Where(x => x.Id == res.Response.Id);
        try
        {
            resultPayment = await clientApi.InitAsync(_iReq, token);
            await q
               .ExecuteUpdateAsync(set => set
                   .SetProperty(p => p.Message, resultPayment.Message)
                   .SetProperty(p => p.Success, resultPayment.Success)
                   .SetProperty(p => p.Details, resultPayment.Details)
                   .SetProperty(p => p.PaymentId, resultPayment.PaymentId)
                   .SetProperty(p => p.ErrorCode, resultPayment.ErrorCode)
                   .SetProperty(p => p.PaymentURL, resultPayment.PaymentURL)
                   .SetProperty(p => p.Status, Enum.Parse<StatusResponsesTBankEnum>(resultPayment.Status.ToString()!))
                   .SetProperty(p => p.TerminalKey, resultPayment.TerminalKey), cancellationToken: token);
        }
        catch (Exception ex)
        {
            res.Messages.InjectException(ex);
            await q
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.ApiException, ex.Message), cancellationToken: token);
            return res;
        }

        if (req.GenerateQR is not null)
        {
            PaymentInitTBankQRModelDB qrDb = new() { TypeQR = req.GenerateQR.Value };
            await ctx.QrForInitPaymentTBank.AddAsync(qrDb, token);
            await ctx.SaveChangesAsync(token);
            await q.ExecuteUpdateAsync(set => set.SetProperty(p => p.PaymentQRId, qrDb.Id), cancellationToken: token);

            GetQr _gq = new(resultPayment.PaymentId)
            {
                DataType = req.GenerateQR?.Convert() ?? TinkoffPaymentClientApi.Enums.EDataTypeQR.PAYLOAD,
                PaymentId = resultPayment.PaymentId,
            };
            QRResponse qrRest;
            try
            {
                qrRest = await clientApi.GetQrAsync(_gq, token);
                await ctx.QrForInitPaymentTBank.ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Success, qrRest.Success)
                    .SetProperty(p => p.DataQR, qrRest.Data)
                    .SetProperty(p => p.Message, qrRest.Message)
                    .SetProperty(p => p.Details, qrRest.Details)
                    .SetProperty(p => p.ErrorCode, qrRest.ErrorCode)
                    .SetProperty(p => p.TerminalKey, qrRest.TerminalKey), cancellationToken: token);
            }
            catch (Exception ex)
            {
                res.Messages.InjectException(ex);
                await ctx.QrForInitPaymentTBank
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.ApiException, ex.Message), cancellationToken: token);

                return res;
            }
        }

        res.Response = await q
            .Include(x => x.PaymentQR)
            .Include(x => x.Receipt)
            .FirstAsync(cancellationToken: token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> IncomingTBankMerchantPaymentAsync(JObject req, CancellationToken token = default)
    {
        TinkoffNotification? tbankNotify = req.ToObject<TinkoffNotification>();

        if (tbankNotify is null)
        {
            loggerRepo.LogError($"TBank payload is null > {req}");
            return ResponseBaseModel.CreateError("TBank payload is null");
        }

        if (!tbankNotify.CheckToken(settings.Value.Password))
        {
            loggerRepo.LogError($"подпись не корректна > {req}");
            return ResponseBaseModel.CreateError("подпись не корректна");
        }

        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        IncomingMerchantPaymentTBankModelDB payDB = tbankNotify.GetDB();
        await ctx.IncomingMerchantsPaymentsTBank.AddAsync(payDB, token);
        await ctx.SaveChangesAsync(token);

        if (!tbankNotify.Amount.HasValue)
        {
            loggerRepo.LogError($"нет суммы > {req}");
            return ResponseBaseModel.CreateError("нет суммы");
        }

        if (string.IsNullOrEmpty(tbankNotify.OrderId) || !int.TryParse(tbankNotify.OrderId, out int orderId))
        {
            loggerRepo.LogError($"не указан orderId (или имеет не верный формат `{tbankNotify.OrderId}`) > {req}");
            return ResponseBaseModel.CreateError("не указан orderId");
        }



        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentInitTBankResultModelDB>> PaymentsInitSelectTBankAsync(TPaginationRequestStandardModel<SelectInitPaymentsTBankRequestModel> req, CancellationToken token = default)
    {
        if (req.PageSize < 10)
            req.PageSize = 10;

        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        IQueryable<PaymentInitTBankResultModelDB> q = ctx.PaymentsInitResultsTBank.AsQueryable();

        return new()
        {
            PageSize = req.PageSize,
            PageNum = req.PageNum,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.Skip(req.PageNum * req.PageSize).Take(req.PageSize).Include(x => x.PaymentQR).Include(x => x.Receipt).ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<IncomingMerchantPaymentTBankModelDB>> IncomingMerchantPaymentsSelectTBankAsync(TPaginationRequestStandardModel<SelectIncomingMerchantPaymentsTBankRequestModel> req, CancellationToken token = default)
    {
        if (req.PageSize < 10)
            req.PageSize = 10;

        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        IQueryable<IncomingMerchantPaymentTBankModelDB> q = ctx.IncomingMerchantsPaymentsTBank.AsQueryable();

        return new()
        {
            PageSize = req.PageSize,
            PageNum = req.PageNum,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }
}