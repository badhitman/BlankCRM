////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using TinkoffPaymentClientApi.ResponseEntity;
using TinkoffPaymentClientApi.Commands;
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
        throw new NotImplementedException();
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

        if (string.IsNullOrEmpty(tbankNotify.OrderId) || !int.TryParse(tbankNotify.OrderId, out int orderId))
        {
            loggerRepo.LogError($"не указан orderId (или имеет не верный формат `{tbankNotify.OrderId}`) > {req}");
            return ResponseBaseModel.CreateError("не указан orderId");
        }

        if (!tbankNotify.Amount.HasValue)
        {
            loggerRepo.LogError($"нет суммы > {req}");
            return ResponseBaseModel.CreateError("нет суммы");
        }

        return ResponseBaseModel.CreateSuccess("Ok");
    }
}