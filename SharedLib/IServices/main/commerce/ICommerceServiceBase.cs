////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public interface ICommerceServiceBase
{
    /// <summary>
    /// Подбор сотрудников (связи пользователей с компаниями)
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>> UsersOrganizationsSelect(TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel> req);

    /// <summary>
    /// UserOrganizationUpdate
    /// </summary>
    public Task<TResponseModel<int>> UserOrganizationUpdate(TAuthRequestModel<UserOrganizationModelDB> req);

    /// <summary>
    /// UsersOrganizationsRead
    /// </summary>
    public Task<TResponseModel<UserOrganizationModelDB[]>> UsersOrganizationsRead(int[] req);

    /// <summary>
    /// Обновление параметров организации. Юридические параметры не меняются, а формируется запрос на изменение, которое должна подтвердить сторонняя система
    /// </summary>
    public Task<TResponseModel<int>> OrganizationUpdate(TAuthRequestModel<OrganizationModelDB> req);

    /// <summary>
    /// Подбор организаций с параметрами запроса
    /// </summary>
    public Task<TPaginationResponseModel<OrganizationModelDB>> OrganizationsSelect(TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req);

    /// <summary>
    /// Прочитать данные организаций по их идентификаторам
    /// </summary>
    public Task<TResponseModel<OrganizationModelDB[]>> OrganizationsRead(int[] req);

    /// <summary>
    /// Обновить/Создать офис/филиал организации
    /// </summary>
    public Task<TResponseModel<int>> OfficeOrganizationUpdate(AddressOrganizationBaseModel req);

    /// <summary>
    /// Установить реквизиты организации (+ сброс запроса редактирования)
    /// </summary>
    /// <remarks>
    /// Если организация находиться в статусе запроса изменения реквизитов - этот признак обнуляется.
    /// </remarks>
    public Task<TResponseModel<bool>> OrganizationSetLegal(OrganizationLegalModel req);

    /// <summary>
    /// Прочитать данные адресов организаций по их идентификаторам
    /// </summary>
    public Task<TResponseModel<OfficeOrganizationModelDB[]>> OfficesOrganizationsRead(int[] ids);

    /// <summary>
    /// Удалить офис/филиал организации
    /// </summary>
    public Task<ResponseBaseModel> OfficeOrganizationDelete(int req);

    /// <summary>
    /// Обновить/создать банковские реквизиты
    /// </summary>
    public Task<TResponseModel<int>> BankDetailsUpdate(TAuthRequestModel<BankDetailsModelDB> req);

    /// <summary>
    /// Удалить банковские реквизиты
    /// </summary>
    public Task<ResponseBaseModel> BankDetailsDelete(TAuthRequestModel<int> req);
}