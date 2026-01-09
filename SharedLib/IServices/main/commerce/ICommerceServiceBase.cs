////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public interface ICommerceServiceBase
{
    /// <summary>
    /// PricesRulesGetForOffers
    /// </summary>
    public Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffersAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// Обновить/создать правило ценообразования
    /// </summary>
    public Task<TResponseModel<int>> PriceRuleUpdateAsync(TAuthRequestStandardModel<PriceRuleForOfferModelDB> price_rule, CancellationToken token = default);

    /// <summary>
    /// Подбор складских документов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseStandardModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// WarehouseDocument update
    /// </summary>
    public Task<TResponseModel<int>> WarehouseDocumentUpdateOrCreateAsync(WarehouseDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// Обновить строку складского документа
    /// </summary>
    public Task<TResponseModel<int>> RowForWarehouseDocumentUpdateAsync(RowOfWarehouseDocumentModelDB row, CancellationToken token = default);

    /// <summary>
    /// Удалить строку складского документа
    /// </summary>
    public Task<RowsForWarehouseDocumentDeleteResponseModel> RowsForWarehouseDocumentDeleteAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Получить остатки
    /// </summary>
    public Task<TPaginationResponseStandardModel<OfferAvailabilityModelDB>> OffersRegistersSelectAsync(TPaginationRequestStandardModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// PaymentDocumentDelete
    /// </summary>
    public Task<ResponseBaseModel> PaymentDocumentDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// IncomingMerchantPaymentTBankAsync
    /// </summary>
    public Task<ResponseBaseModel> IncomingMerchantPaymentTBankAsync(IncomingMerchantPaymentTBankNotifyModel req, CancellationToken token = default);

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

    /// <summary>
    /// Прочитать недельные расписания (по идентификаторам)
    /// </summary>
    public Task<List<WeeklyScheduleModelDB>> WeeklySchedulesReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Расписание на дату создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> CalendarScheduleUpdateOrCreateAsync(TAuthRequestStandardModel<CalendarScheduleModelDB> work, CancellationToken token = default);

    /// <summary>
    /// Подбор расписаний для дат
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<CalendarScheduleModelDB>>> CalendarsSchedulesSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<WorkScheduleCalendarsSelectRequestModel>> req, CancellationToken token = default);


    /// <summary>
    /// Расписания для дат прочитать по их идентификаторам
    /// </summary>
    public Task<TResponseModel<List<CalendarScheduleModelDB>>> CalendarsSchedulesReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// Обновить/создать платёжный документ
    /// </summary>
    public Task<TResponseModel<int>> PaymentDocumentUpdateAsync(TAuthRequestStandardModel<PaymentDocumentBaseModel> payment, CancellationToken token = default);

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