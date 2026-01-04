////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public interface ICommerceServiceBase
{
    /// <summary>
    /// Price Full - file get
    /// </summary>
    public Task<FileAttachModel> PriceFullFileGetExcelAsync(CancellationToken token = default);

    /// <summary>
    /// WarehouseDocuments read
    /// </summary>
    public Task<TResponseModel<WarehouseDocumentModelDB[]>> WarehousesDocumentsReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Price Full - file get
    /// </summary>
    public Task<FileAttachModel> PriceFullFileGetJsonAsync(CancellationToken token = default);

    /// <summary>
    /// UploadOffersAsync
    /// </summary>
    public Task<ResponseBaseModel> UploadOffersAsync(List<NomenclatureScopeModel> req, CancellationToken token = default);

    /// <summary>
    /// Подбор сотрудников (связи пользователей с компаниями)
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>> UsersOrganizationsSelectAsync(TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// UserOrganizationUpdate
    /// </summary>
    public Task<TResponseModel<int>> UserOrganizationUpdateAsync(TAuthRequestModel<UserOrganizationModelDB> req, CancellationToken token = default);

    /// <summary>
    /// UsersOrganizationsRead
    /// </summary>
    public Task<TResponseModel<UserOrganizationModelDB[]>> UsersOrganizationsReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Обновление параметров организации. Юридические параметры не меняются, а формируется запрос на изменение, которое должна подтвердить сторонняя система
    /// </summary>
    public Task<TResponseModel<int>> OrganizationUpdateAsync(TAuthRequestModel<OrganizationModelDB> req, CancellationToken token = default);

    /// <summary>
    /// Подбор организаций с параметрами запроса
    /// </summary>
    public Task<TPaginationResponseModel<OrganizationModelDB>> OrganizationsSelectAsync(TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные организаций по их идентификаторам
    /// </summary>
    public Task<TResponseModel<OrganizationModelDB[]>> OrganizationsReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Обновить/Создать офис/филиал организации
    /// </summary>
    public Task<TResponseModel<int>> OfficeOrganizationUpdateAsync(AddressOrganizationBaseModel req, CancellationToken token = default);

    /// <summary>
    /// Установить реквизиты организации (+ сброс запроса редактирования)
    /// </summary>
    /// <remarks>
    /// Если организация находиться в статусе запроса изменения реквизитов - этот признак обнуляется.
    /// </remarks>
    public Task<TResponseModel<bool>> OrganizationSetLegalAsync(OrganizationLegalModel req, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные адресов организаций по их идентификаторам
    /// </summary>
    public Task<TResponseModel<OfficeOrganizationModelDB[]>> OfficesOrganizationsReadAsync(int[] ids, CancellationToken token = default);

    /// <summary>
    /// Удалить офис/филиал организации
    /// </summary>
    public Task<ResponseBaseModel> OfficeOrganizationDeleteAsync(int req, CancellationToken token = default);

    /// <summary>
    /// Обновить/создать банковские реквизиты
    /// </summary>
    public Task<TResponseModel<int>> BankDetailsUpdateAsync(TAuthRequestModel<BankDetailsModelDB> req, CancellationToken token = default);

    /// <summary>
    /// Удалить банковские реквизиты
    /// </summary>
    public Task<ResponseBaseModel> BankDetailsDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default);
}