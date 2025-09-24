////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SharedLib;

namespace ApiRestService.Controllers;

/// <summary>
/// Платежи
/// </summary>
[Route("api/[controller]/[action]"), ApiController, ServiceFilter(typeof(UnhandledExceptionAttribute))]
[TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.MerchantIncomingCommerce)},{nameof(ExpressApiRolesEnum.PaymentsWriteCommerce)}"])]
public class MerchantsController(IMerchantService merchantRepo) : ControllerBase
{
    /// <summary>
    /// Incoming payment `Merchant of TBank`
    /// </summary>
    [HttpPut($"/api/{Routes.MERCHANT_CONTROLLER_NAME}/{Routes.BANK_CONTROLLER_NAME}-{nameof(MerchantsBanksEnum.TBankCash)}/{Routes.INCOMING_CONTROLLER_NAME}-{Routes.PAYMENT_CONTROLLER_NAME}")]
    public async Task<ResponseBaseModel> IncomingTBankMerchantPaymentAsync(JObject req) => await merchantRepo.IncomingTBankMerchantPaymentAsync(req);
}