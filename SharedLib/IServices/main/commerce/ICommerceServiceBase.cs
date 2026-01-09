////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public partial interface ICommerceServiceBase
{
    /// <summary>
    /// Get order report file Excel (*.xlsx)
    /// </summary>
    public Task<TResponseModel<FileAttachModel>> GetOrderReportFileAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Обновить/Создать товар
    /// </summary>
    public Task<TResponseModel<int>> NomenclatureUpdateAsync(NomenclatureModelDB req, CancellationToken token = default);

    /// <summary>
    /// NomenclaturesSelect
    /// </summary>
    public Task<TPaginationResponseStandardModel<NomenclatureModelDB>> NomenclaturesSelectAsync(TPaginationRequestStandardModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Получить заказы (по заявкам)
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesGetAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Подбор заказов (поиск по параметрам)
    /// </summary>
    public Task<TPaginationResponseStandardModel<OrderDocumentModelDB>> OrdersSelectAsync(TPaginationRequestStandardModel<TAuthRequestStandardModel<OrdersSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Order update
    /// </summary>
    public Task<TResponseModel<int>> OrderUpdateAsync(OrderDocumentModelDB req, CancellationToken token = default);

    /// <summary>
    /// Orders read
    /// </summary>
    public Task<TResponseModel<OrderDocumentModelDB[]>> OrdersReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// Обновить строку заказа
    /// </summary>
    public Task<TResponseModel<int>> RowForOrderUpdateAsync(RowOfOrderDocumentModelDB row, CancellationToken token = default);

    /// <summary>
    /// Удалить строку заказа
    /// </summary>
    public Task<TResponseModel<bool>> RowsForOrderDeleteAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// NomenclaturesRead
    /// </summary>
    public Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);

    /// <summary>
    /// Удалить ценообразование
    /// </summary>
    public Task<ResponseBaseModel> PriceRuleDeleteAsync(TAuthRequestStandardModel<int> id, CancellationToken token = default);

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
}