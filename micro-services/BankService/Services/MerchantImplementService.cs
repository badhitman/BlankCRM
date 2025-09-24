////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using TinkoffPaymentClientApi.ResponseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SharedLib;
using DbcLib;

namespace BankService;

/// <summary>
/// MerchantImplementService
/// </summary>
public partial class MerchantImplementService(IOptions<TBankSettings> settings, ILogger<MerchantImplementService> loggerRepo, IDbContextFactory<BankContext> bankDbFactory)
    : IMerchantService
{
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

        if (string.IsNullOrEmpty(tbankNotify.OrderId))
        {
            loggerRepo.LogError($"не указан orderId > {req}");
            return ResponseBaseModel.CreateError("не указан orderId");
        }

        if (!tbankNotify.Amount.HasValue)
        {
            loggerRepo.LogError($"нет суммы > {req}");
            return ResponseBaseModel.CreateError("нет суммы");
        }

        if (!int.TryParse(tbankNotify.OrderId, out int orderId))
        {
            loggerRepo.LogError($"не удалось преобразовать `{nameof(orderId)}` = '{orderId}' > {req}");
            return ResponseBaseModel.CreateError($"не удалось преобразовать `{nameof(orderId)}` = '{orderId}'");
        }



        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }
}