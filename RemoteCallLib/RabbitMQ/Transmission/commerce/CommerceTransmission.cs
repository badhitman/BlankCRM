////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// CommerceTransmission
/// </summary>
public partial class CommerceTransmission(IRabbitClient rabbitClient) : ICommerceTransmission
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RecordsAttendanceModelDB>> RecordsAttendancesSelectAsync(TPaginationRequestAuthModel<RecordsAttendancesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<RecordsAttendanceModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.RecordsAttendancesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateAttendanceRecordsAsync(TAuthRequestStandardModel<CreateAttendanceRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CreateAttendanceRecordsCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RecordsAttendanceModelDB>>> StatusesOrdersAttendancesChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RecordsAttendanceModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.StatusesOrdersAttendancesChangeByHelpDeskDocumentIdReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> OrganizationOfferContractUpdateAsync(TAuthRequestStandardModel<OrganizationOfferToggleModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationOfferContractUpdateOrCreateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<WorksFindResponseModel> WorksSchedulesFindAsync(WorkFindRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<WorksFindResponseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.WorksSchedulesFindCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<NomenclatureModelDB>> NomenclaturesSelectAsync(TPaginationRequestStandardModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<NomenclatureModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.NomenclaturesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OfficeOrganizationDeleteAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OfficeOrganizationDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfficeOrganizationUpdateAsync(AddressOrganizationBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.OfficeOrganizationUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> NomenclatureUpdateOrCreateAsync(NomenclatureModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.NomenclatureUpdateOrCreateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<OfferModelDB>>> OffersSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<OffersSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseStandardModel<OfferModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.OfferSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfferUpdateOrCreateAsync(TAuthRequestStandardModel<OfferModelDB> offer, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.OfferUpdateOrCreateCommerceReceive, offer, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>> OrdersReadAsync(TAuthRequestStandardModel<int[]> orders_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrderDocumentModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrdersReadCommerceReceive, orders_ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> OrganizationSetLegalAsync(OrganizationLegalModel org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationSetLegalCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrganizationModelDB[]>> OrganizationsReadAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrganizationModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationsReadCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OrganizationModelDB>> OrganizationsSelectAsync(TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<OrganizationModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationsSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OrganizationUpdateAsync(TAuthRequestStandardModel<OrganizationModelDB> org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationUpdateOrCreateCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PaymentDocumentDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PaymentDocumentDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RowForOrderUpdateOrCreateAsync(RowOfOrderDocumentModelDB row, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.RowForOrderUpdateOrCreateCommerceReceive, row, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RowOrderDocumentRecord[]>> RowsDeleteFromOrderAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<RowOrderDocumentRecord[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.RowsDeleteFromOrderCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OfficeOrganizationModelDB[]>> OfficesOrganizationsReadAsync(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OfficeOrganizationModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OfficesOrganizationsReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesReadAsync(TAuthRequestStandardModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<NomenclatureModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.NomenclaturesReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OfferModelDB[]>> OffersReadAsync(TAuthRequestStandardModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OfferModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OfferReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffersAsync(TAuthRequestStandardModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<PriceRuleForOfferModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.PricesRulesGetForOfferCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OrderDocumentModelDB>> OrdersSelectAsync(TPaginationRequestStandardModel<TAuthRequestStandardModel<OrdersSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<OrderDocumentModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrdersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OrderUpdateOrCreateAsync(OrderDocumentModelDB order, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderUpdateOrCreateCommerceReceive, order, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PaymentDocumentUpdateAsync(TAuthRequestStandardModel<PaymentDocumentBaseModel> payment, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.PaymentDocumentUpdateCommerceReceive, payment, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PriceRuleUpdateAsync(TAuthRequestStandardModel<PriceRuleForOfferModelDB> price_rule, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.PriceRuleUpdateCommerceReceive, price_rule, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PriceRuleDeleteAsync(TAuthRequestStandardModel<int> id, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<PriceRuleForOfferModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.PriceRuleDeleteCommerceReceive, id, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StatusOrderChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.StatusOrderChangeByHelpDeskDocumentIdReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesGetAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrderDocumentModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrdersByIssuesGetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FileAttachModel>> GetOrderReportFileAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FileAttachModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderReportGetCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<FileAttachModel> PriceFullFileGetExcelAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<FileAttachModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PriceFullFileGetExcelCommerceReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<FileAttachModel> PriceFullFileGetJsonAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<FileAttachModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PriceFullFileGetJsonCommerceReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WeeklyScheduleCreateOrUpdateAsync(WeeklyScheduleModelDB work, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.WeeklyScheduleCreateOrUpdateReceive, work, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WeeklyScheduleModelDB>> WeeklySchedulesSelectAsync(TPaginationRequestStandardModel<WorkSchedulesSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<WeeklyScheduleModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.WeeklySchedulesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<WeeklyScheduleModelDB>> WeeklySchedulesReadAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<WeeklyScheduleModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.WeeklySchedulesReadCommerceReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CalendarScheduleUpdateOrCreateAsync(TAuthRequestStandardModel<CalendarScheduleModelDB> work, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.CalendarScheduleUpdateOrCreateCommerceReceive, work, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<CalendarScheduleModelDB>>> CalendarsSchedulesSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<WorkScheduleCalendarsSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseStandardModel<CalendarScheduleModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.CalendarsSchedulesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CalendarScheduleModelDB>>> CalendarsSchedulesReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<CalendarScheduleModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.CalendarsSchedulesReadCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<UserOrganizationModelDB>>> UsersOrganizationsSelectAsync(TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseStandardModel<UserOrganizationModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationsUsersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> UserOrganizationUpdateAsync(TAuthRequestStandardModel<UserOrganizationModelDB> org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationUserUpdateOrCreateCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserOrganizationModelDB[]>> UsersOrganizationsReadAsync(int[] organizations_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserOrganizationModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationsUsersReadCommerceReceive, organizations_ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RecordsAttendanceModelDB[]>> RecordsAttendancesByIssuesGetAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<RecordsAttendanceModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrdersAttendancesByIssuesGetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RecordsAttendanceModelDB[]>> AttendanceRecordsDeleteAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<RecordsAttendanceModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.AttendanceRecordDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankDetailsUpdateAsync(TAuthRequestStandardModel<BankDetailsModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.BankDetailsUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> BankDetailsDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.BankDetailsDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> IncomingMerchantPaymentTBankAsync(IncomingMerchantPaymentTBankNotifyModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.IncomingMerchantPaymentTBankReceive, req, false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UploadOffersAsync(List<NomenclatureScopeModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UploadOffersCommerceReceive, req, token: token) ?? new();
}