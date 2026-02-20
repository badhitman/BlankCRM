////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace ApiRestService.Controllers;

/// <summary>
/// Offers
/// </summary>
[Route("api/[controller]/[action]"), ApiController, ServiceFilter(typeof(UnhandledExceptionAttribute))]
[TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersReadCommerce)},{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
public class OffersController(ICommerceTransmission commRepo) : ControllerBase
{
    /// <summary>
    /// Подбор офферов (поиск по параметрам)
    /// </summary>
    /// <remarks>
    /// Роли: <see cref="ExpressApiRolesEnum.OrdersReadCommerce"/>, <see cref="ExpressApiRolesEnum.OrdersWriteCommerce"/>
    /// </remarks>
    [HttpPut($"/api/{Routes.OFFERS_CONTROLLER_NAME}/{Routes.SELECT_ACTION_NAME}")]
#if !DEBUG
    [LoggerNolog]
#endif
    public async Task<TResponseModel<TPaginationResponseStandardModel<OfferModelDB>>> OffersSelect(TPaginationRequestStandardModel<OffersSelectRequestModel> req)
        => await commRepo.OffersSelectAsync(new() { Payload = req, SenderActionUserId = GlobalStaticConstantsRoles.Roles.System });

    /// <summary>
    /// Чтение данных офферов (по идентификаторам)
    /// </summary>
    /// <remarks>
    /// Роли: <see cref="ExpressApiRolesEnum.OrdersReadCommerce"/>, <see cref="ExpressApiRolesEnum.OrdersWriteCommerce"/>
    /// </remarks>
    [HttpPut($"/api/{Routes.OFFERS_CONTROLLER_NAME}/{Routes.READ_ACTION_NAME}")]
#if !DEBUG
    [LoggerNolog]
#endif
    public async Task<TResponseModel<OfferModelDB[]>> OffersRead(int[] req)
        => await commRepo.OffersReadAsync(new() { Payload = req, SenderActionUserId = GlobalStaticConstantsRoles.Roles.System });
}