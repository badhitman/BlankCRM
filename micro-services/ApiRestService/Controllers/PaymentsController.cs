﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib;

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
    [HttpPost($"/{GlobalStaticConstants.Routes.API_CONTROLLER_NAME}/{GlobalStaticConstants.Routes.PAYMENT_CONTROLLER_NAME}/{GlobalStaticConstants.Routes.UPDATE_ACTION_NAME}"), LoggerLog]
    [TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
    public async Task<TResponseModel<int>> PaymentDocumentUpdate(PaymentDocumentBaseModel payment)
        => await commRepo.PaymentDocumentUpdateAsync(new() { Payload = payment, SenderActionUserId = GlobalStaticConstants.Roles.System });

    /// <summary>
    /// Удалить платёжный документ
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.PaymentsWriteCommerce"/>
    /// </remarks>
    [HttpDelete($"/{GlobalStaticConstants.Routes.API_CONTROLLER_NAME}/{GlobalStaticConstants.Routes.PAYMENT_CONTROLLER_NAME}/{GlobalStaticConstants.Routes.DELETE_ACTION_NAME}/{{payment_id}}"), LoggerLog]
    [TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
    public async Task<ResponseBaseModel> PaymentDocumentDelete([FromRoute] int payment_id)
        => await commRepo.PaymentDocumentDeleteAsync(new() { Payload = payment_id, SenderActionUserId = GlobalStaticConstants.Roles.System });
}