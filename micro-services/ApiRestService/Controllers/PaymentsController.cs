////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace ApiRestService.Controllers;

/// <summary>
/// Платежи
/// </summary>
[Route("api/[controller]/[action]"), ApiController, ServiceFilter(typeof(UnhandledExceptionAttribute)), LoggerNolog]
[TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersReadCommerce)},{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
public class PaymentsController(ICommerceTransmission commRepo) : ControllerBase
{
    /// <summary>
    /// Обновить/создать платёжный документ
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.PaymentsWriteCommerce"/>
    /// </remarks>
    [HttpPost($"/{Routes.API_CONTROLLER_NAME}/{Routes.PAYMENT_CONTROLLER_NAME}/{Routes.UPDATE_ACTION_NAME}"), LoggerLog]
    [TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
    public async Task<TResponseModel<int>> PaymentDocumentUpdate(PaymentDocumentBaseModel payment)
        => await commRepo.PaymentDocumentUpdateAsync(new() { Payload = payment, SenderActionUserId = GlobalStaticConstantsRoles.Roles.System });

    /// <summary>
    /// Удалить платёжный документ
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.PaymentsWriteCommerce"/>
    /// </remarks>
    [HttpDelete($"/{Routes.API_CONTROLLER_NAME}/{Routes.PAYMENT_CONTROLLER_NAME}/{Routes.DELETE_ACTION_NAME}/{{payment_id}}"), LoggerLog]
    [TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
    public async Task<ResponseBaseModel> PaymentDocumentDelete([FromRoute] int payment_id)
        => await commRepo.PaymentDocumentDeleteAsync(new() { Payload = payment_id, SenderActionUserId = GlobalStaticConstantsRoles.Roles.System });
}