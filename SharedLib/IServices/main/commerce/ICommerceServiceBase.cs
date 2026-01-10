////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public partial interface ICommerceServiceBase
{   
    /// <summary>
    /// Удалить ценообразование
    /// </summary>
    public Task<ResponseBaseModel> PriceRuleDeleteAsync(TAuthRequestStandardModel<int> id, CancellationToken token = default);

    /// <summary>
    /// Обновить/создать правило ценообразования
    /// </summary>
    public Task<TResponseModel<int>> PriceRuleUpdateAsync(TAuthRequestStandardModel<PriceRuleForOfferModelDB> price_rule, CancellationToken token = default);

    /// <summary>
    /// PaymentDocumentDelete
    /// </summary>
    public Task<ResponseBaseModel> PaymentDocumentDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// IncomingMerchantPaymentTBankAsync
    /// </summary>
    public Task<ResponseBaseModel> IncomingMerchantPaymentTBankAsync(IncomingMerchantPaymentTBankNotifyModel req, CancellationToken token = default);

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
    /// Price Full - file get
    /// </summary>
    public Task<FileAttachModel> PriceFullFileGetJsonAsync(CancellationToken token = default);
}