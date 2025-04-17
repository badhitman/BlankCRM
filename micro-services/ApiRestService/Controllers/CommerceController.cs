////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace ApiRestService.Controllers;

/// <summary>
/// Номенклатура
/// </summary>
[Route("api/[controller]/[action]"), ApiController, ServiceFilter(typeof(UnhandledExceptionAttribute))]
[TypeFilter(typeof(RolesAuthorizationFilter), Arguments = [$"{nameof(ExpressApiRolesEnum.OrdersReadCommerce)},{nameof(ExpressApiRolesEnum.OrdersWriteCommerce)}"])]
public class CommerceController(ICommerceTransmission commRepo) : ControllerBase
{
    /// <summary>
    /// Подбор номенклатуры (поиск по параметрам)
    /// </summary>
    /// <remarks>
    /// Роли: <see cref="ExpressApiRolesEnum.OrdersReadCommerce"/>, <see cref="ExpressApiRolesEnum.OrdersWriteCommerce"/>
    /// </remarks>
    [HttpPut($"/api/{Routes.COMMERCE_CONTROLLER_NAME}/{Routes.ORDERS_CONTROLLER_NAME}-{Routes.SELECT_ACTION_NAME}")]
#if !DEBUG
    [LoggerNolog]
#endif
    public async Task<TPaginationResponseModel<NomenclatureModelDB>> OrdersSelect(TPaginationRequestModel<NomenclaturesSelectRequestModel> req)
        => await commRepo.NomenclaturesSelectAsync(req);

    /// <summary>
    /// Чтение номенклатуры (по идентификаторам)
    /// </summary>
    /// <remarks>
    /// Роли: <see cref="ExpressApiRolesEnum.OrdersReadCommerce"/>, <see cref="ExpressApiRolesEnum.OrdersWriteCommerce"/>
    /// </remarks>
    [HttpPut($"/api/{Routes.COMMERCE_CONTROLLER_NAME}/{Routes.NOMENCLATURES_CONTROLLER_NAME}-{Routes.READ_ACTION_NAME}")]
#if !DEBUG
    [LoggerNolog]
#endif
    public async Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesRead(int[] req)
        => await commRepo.NomenclaturesReadAsync(new() { Payload = req, SenderActionUserId = GlobalStaticConstants.Roles.System });
}