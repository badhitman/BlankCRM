////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace ApiRestService.Controllers;

/// <summary>
/// Платежи
/// </summary>
[Route("api/[controller]/[action]"), ApiController, ServiceFilter(typeof(UnhandledExceptionAttribute))]
[TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.PaymentsWriteCommerce)},{nameof(ExpressApiRolesEnum.PaymentsReadCommerce)}"])]
public class PaymentsController(ICommerceTransmission commRepo) : ControllerBase
{
    /// <summary>
    /// Обновить/создать платёжный документ
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.PaymentsWriteCommerce"/>
    /// </remarks>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.PAYMENT_CONTROLLER_NAME}/{Routes.UPDATE_ACTION_NAME}"), LoggerLog]
    [TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.PaymentsWriteCommerce)}"])]
    public async Task<TResponseModel<int>> PaymentDocumentUpdate(PaymentDocumentBaseModel payment)
        => await commRepo.PaymentDocumentUpdateOrCreateAsync(new() { Payload = payment, SenderActionUserId = GlobalStaticConstantsRoles.Roles.System });
}