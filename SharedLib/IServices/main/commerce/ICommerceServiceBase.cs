////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public interface ICommerceServiceCore
{
    /// <summary>
    /// Подбор записей (актуальных)
    /// </summary>
    public Task<TPaginationResponseStandardModel<RecordsAttendanceModelDB>> RecordsAttendancesSelectAsync(TPaginationRequestAuthModel<RecordsAttendancesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Удалить запись/бронь
    /// </summary>
    public Task<ResponseBaseModel> AttendanceRecordsDeleteAsync(TAuthRequestStandardModel<int> orderId, CancellationToken token = default);
    
    /// <summary>
    /// Получить заказы (по заявкам)
    /// </summary>
    public Task<TResponseModel<RecordsAttendanceModelDB[]>> RecordsAttendancesByIssuesGetAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Создать пакет записей/броней
    /// </summary>
    /// <remarks>
    /// Бронирует свободные слоты
    /// </remarks>
    public Task<ResponseBaseModel> CreateAttendanceRecordsAsync(TAuthRequestStandardModel<CreateAttendanceRequestModel> workSchedules, CancellationToken token = default);
    
    /// <summary>
    /// OrganizationOfferContractUpdate
    /// </summary>
    public Task<TResponseModel<bool>> OrganizationOfferContractUpdateAsync(TAuthRequestStandardModel<OrganizationOfferToggleModel> req, CancellationToken token = default);

    /// <summary>
    /// Недельное расписание обновить/создать
    /// </summary>
    public Task<TResponseModel<int>> WeeklyScheduleUpdateAsync(WeeklyScheduleModelDB work, CancellationToken token = default);

    /// <summary>
    /// Подбор недельных расписаний
    /// </summary>
    public Task<TPaginationResponseStandardModel<WeeklyScheduleModelDB>> WeeklySchedulesSelectAsync(TPaginationRequestStandardModel<WorkSchedulesSelectRequestModel> req, CancellationToken token = default);
}


/// <inheritdoc/>
public interface ICommerceServiceBase : ICommerceServiceCore
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
    /// Загрузка справочника номенклатуры/офферов
    /// </summary>
    public Task<ResponseBaseModel> UploadOffersAsync(List<NomenclatureScopeModel> req, CancellationToken token = default);

    /// <summary>
    /// Подбор сотрудников (связи пользователей с компаниями)
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<UserOrganizationModelDB>>> UsersOrganizationsSelectAsync(TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// UserOrganizationUpdate
    /// </summary>
    public Task<TResponseModel<int>> UserOrganizationUpdateAsync(TAuthRequestStandardModel<UserOrganizationModelDB> req, CancellationToken token = default);

    /// <summary>
    /// UsersOrganizationsRead
    /// </summary>
    public Task<TResponseModel<UserOrganizationModelDB[]>> UsersOrganizationsReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Обновление параметров организации. Юридические параметры не меняются, а формируется запрос на изменение, которое должна подтвердить сторонняя система
    /// </summary>
    public Task<TResponseModel<int>> OrganizationUpdateAsync(TAuthRequestStandardModel<OrganizationModelDB> req, CancellationToken token = default);

    /// <summary>
    /// Подбор организаций с параметрами запроса
    /// </summary>
    public Task<TPaginationResponseStandardModel<OrganizationModelDB>> OrganizationsSelectAsync(TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req, CancellationToken token = default);

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
    public Task<TResponseModel<int>> BankDetailsUpdateAsync(TAuthRequestStandardModel<BankDetailsModelDB> req, CancellationToken token = default);

    /// <summary>
    /// Удалить банковские реквизиты
    /// </summary>
    public Task<ResponseBaseModel> BankDetailsDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
}