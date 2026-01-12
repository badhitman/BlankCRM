////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using static SharedLib.GlobalStaticConstantsRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ApiRestService.Controllers;

/// <summary>
/// Организации
/// </summary>
[Route("api/[controller]/[action]"), ApiController, ServiceFilter(typeof(UnhandledExceptionAttribute))]
#if DEBUG
[AllowAnonymous]
#else
[Authorize(Roles = $"{nameof(ExpressApiRolesEnum.OrganizationsReadCommerce)},{nameof(ExpressApiRolesEnum.OrganizationsWriteCommerce)}"), LoggerNolog]
#endif
public class OrganizationsController(ICommerceTransmission commRepo) : ControllerBase
{
    /// <summary>
    /// Прочитать данные организаций по их идентификаторам
    /// </summary>
    /// <remarks>
    /// Роли: <see cref="ExpressApiRolesEnum.OrganizationsReadCommerce"/>, <see cref="ExpressApiRolesEnum.OrganizationsWriteCommerce"/>
    /// </remarks>
    [HttpPut($"/api/{Routes.ORGANIZATIONS_CONTROLLER_NAME}/{Routes.READ_ACTION_NAME}")]
    public async Task<TResponseModel<OrganizationModelDB[]>> ReadOrganizations(int[] organizations_ids)
        => await commRepo.OrganizationsReadAsync(organizations_ids);

    /// <summary>
    /// Подбор организаций с параметрами запроса
    /// </summary>
    /// <remarks>
    /// Роли: <see cref="ExpressApiRolesEnum.OrganizationsReadCommerce"/>, <see cref="ExpressApiRolesEnum.OrganizationsWriteCommerce"/>
    /// </remarks>
    [HttpPut($"/api/{Routes.ORGANIZATIONS_CONTROLLER_NAME}/{Routes.SELECT_ACTION_NAME}")]
    public async Task<TPaginationResponseStandardModel<OrganizationModelDB>> OrganizationsSelect(TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req)
        => await commRepo.OrganizationsSelectAsync(req);

    /// <summary>
    /// Прочитать данные адресов организаций по их идентификаторам
    /// </summary>
    /// <remarks>
    /// Роли: <see cref="ExpressApiRolesEnum.OrganizationsReadCommerce"/>, <see cref="ExpressApiRolesEnum.OrganizationsWriteCommerce"/>
    /// </remarks>
    [HttpPut($"/api/{Routes.ORGANIZATIONS_CONTROLLER_NAME}/{Routes.ADDRESSES_CONTROLLER_NAME}-{Routes.READ_ACTION_NAME}")]
    public async Task<TResponseModel<OfficeOrganizationModelDB[]>> OfficesOrganizationsRead(int[] ids)
        => await commRepo.OfficesOrganizationsReadAsync(ids);


    /// <summary>
    /// Установить реквизиты организации (+ сброс запроса редактирования)
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.OrganizationsWriteCommerce"/>.
    /// Если организация находиться в статусе запроса изменения реквизитов - этот признак обнуляется.
    /// </remarks>
    [HttpPost($"/api/{Routes.ORGANIZATIONS_CONTROLLER_NAME}/{Routes.LEGAL_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}"), LoggerLog]
#if DEBUG
    [AllowAnonymous]
#else
[Authorize(Roles = nameof(ExpressApiRolesEnum.OrganizationsWriteCommerce))]
#endif
    public async Task<TResponseModel<bool>> OrganizationSetLegal(OrganizationLegalModel org)
        => await commRepo.OrganizationSetLegalAsync(org);

    /// <summary>
    /// Обновление параметров организации. Юридические параметры не меняются, а формируется запрос на изменение, которое должна подтвердить сторонняя система
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.OrganizationsWriteCommerce"/>
    /// </remarks>
    [HttpPost($"/api/{Routes.ORGANIZATIONS_CONTROLLER_NAME}/{Routes.UPDATE_ACTION_NAME}"), LoggerLog]
#if DEBUG
    [AllowAnonymous]
#else
[Authorize(Roles = nameof(ExpressApiRolesEnum.OrganizationsWriteCommerce))]
#endif
    public async Task<TResponseModel<int>> OrganizationUpdate(OrganizationModelDB org)
        => await commRepo.OrganizationUpdateAsync(new() { Payload = org, SenderActionUserId = Roles.System });

    /// <summary>
    /// Обновить/Создать адрес организации
    /// </summary>
    /// <remarks>
    /// Роль: <see cref="ExpressApiRolesEnum.OrganizationsWriteCommerce"/>
    /// </remarks>
    [HttpPost($"/api/{Routes.ORGANIZATIONS_CONTROLLER_NAME}/{Routes.OFFICE_CONTROLLER_NAME}-{Routes.UPDATE_ACTION_NAME}"), LoggerLog]
#if DEBUG
    [AllowAnonymous]
#else
[Authorize(Roles = nameof(ExpressApiRolesEnum.OrganizationsWriteCommerce))]
#endif
    public async Task<TResponseModel<int>> OfficeOrganizationUpdate(AddressOrganizationBaseModel req)
        => await commRepo.OfficeOrganizationUpdateOrCreateAsync(new()
        {
            Payload = req,
            SenderActionUserId = Roles.System,
        });
}