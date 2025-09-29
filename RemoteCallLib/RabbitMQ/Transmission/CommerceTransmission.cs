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
    public async Task<TPaginationResponseModel<RecordsAttendanceModelDB>> RecordsAttendancesSelectAsync(TPaginationRequestAuthModel<RecordsAttendancesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RecordsAttendanceModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.RecordsAttendancesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateAttendanceRecordsAsync(TAuthRequestModel<CreateAttendanceRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CreateAttendanceRecordsCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusesOrdersAttendancesChangeByHelpDeskDocumentIdAsync(TAuthRequestModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrdersAttendancesStatusesChangeByHelpDeskDocumentIdReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> OrganizationOfferContractUpdateAsync(TAuthRequestModel<OrganizationOfferToggleModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationOfferContractUpdateOrCreateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<WorksFindResponseModel> WorksSchedulesFindAsync(WorkFindRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<WorksFindResponseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.WorksSchedulesFindCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NomenclatureModelDB>> NomenclaturesSelectAsync(TPaginationRequestStandardModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<NomenclatureModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.NomenclaturesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OfficeOrganizationDeleteAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OfficeOrganizationDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfficeOrganizationUpdateAsync(AddressOrganizationBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.OfficeOrganizationUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> NomenclatureUpdateAsync(NomenclatureModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.NomenclatureUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OfferDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OfferDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<OfferModelDB>>> OffersSelectAsync(TAuthRequestModel<TPaginationRequestStandardModel<OffersSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<OfferModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.OfferSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfferUpdateAsync(TAuthRequestModel<OfferModelDB> offer, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.OfferUpdateCommerceReceive, offer, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>> OrdersReadAsync(TAuthRequestModel<int[]> orders_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrderDocumentModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrdersReadCommerceReceive, orders_ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> OrganizationSetLegalAsync(OrganizationLegalModel org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationSetLegalCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrganizationModelDB[]>> OrganizationsReadAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrganizationModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationsReadCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrganizationModelDB>> OrganizationsSelectAsync(TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<OrganizationModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationsSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OrganizationUpdateAsync(TAuthRequestModel<OrganizationModelDB> org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationUpdateOrCreateCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PaymentDocumentDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PaymentDocumentDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RowForOrderUpdateAsync(RowOfOrderDocumentModelDB row, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.RowForOrderUpdateCommerceReceive, row, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> RowsForOrderDeleteAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.RowsDeleteFromOrderCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OfficeOrganizationModelDB[]>> OfficesOrganizationsReadAsync(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OfficeOrganizationModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OfficesOrganizationsReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesReadAsync(TAuthRequestModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<NomenclatureModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.NomenclaturesReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OfferModelDB[]>> OffersReadAsync(TAuthRequestModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OfferModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OfferReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffersAsync(TAuthRequestModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<PriceRuleForOfferModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.PricesRulesGetForOfferCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrderDocumentModelDB>> OrdersSelectAsync(TPaginationRequestStandardModel<TAuthRequestModel<OrdersSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<OrderDocumentModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrdersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OrderUpdateAsync(OrderDocumentModelDB order, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderUpdateCommerceReceive, order, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PaymentDocumentUpdateAsync(TAuthRequestModel<PaymentDocumentBaseModel> payment, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.PaymentDocumentUpdateCommerceReceive, payment, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PriceRuleUpdateAsync(TAuthRequestModel<PriceRuleForOfferModelDB> price_rule, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.PriceRuleUpdateCommerceReceive, price_rule, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PriceRuleDeleteAsync(TAuthRequestModel<int> id, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.PriceRuleDeleteCommerceReceive, id, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusOrderChangeByHelpDeskDocumentIdAsync(TAuthRequestModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.StatusChangeOrderByHelpDeskDocumentIdReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrderDocumentModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrdersByIssuesGetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FileAttachModel>> OrderReportGetAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FileAttachModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderReportGetCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<FileAttachModel> PriceFullFileGetAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<FileAttachModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PriceFullFileGetCommerceReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WeeklyScheduleUpdateAsync(WeeklyScheduleModelDB work, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.WeeklyScheduleUpdateCommerceReceive, work, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WeeklyScheduleModelDB>> WeeklySchedulesSelectAsync(TPaginationRequestStandardModel<WorkSchedulesSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WeeklyScheduleModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.WeeklySchedulesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<WeeklyScheduleModelDB>> WeeklySchedulesReadAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<WeeklyScheduleModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.WeeklySchedulesReadCommerceReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CalendarScheduleUpdateAsync(TAuthRequestModel<CalendarScheduleModelDB> work, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.CalendarScheduleUpdateCommerceReceive, work, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>> CalendarsSchedulesSelectAsync(TAuthRequestModel<TPaginationRequestStandardModel<WorkScheduleCalendarsSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.CalendarsSchedulesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CalendarScheduleModelDB>>> CalendarsSchedulesReadAsync(TAuthRequestModel<int[]> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<CalendarScheduleModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.CalendarsSchedulesReadCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>> UsersOrganizationsSelectAsync(TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationsUsersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> UserOrganizationUpdateAsync(TAuthRequestModel<UserOrganizationModelDB> org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationUserUpdateOrCreateCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserOrganizationModelDB[]>> UsersOrganizationsReadAsync(int[] organizations_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserOrganizationModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationsUsersReadCommerceReceive, organizations_ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RecordsAttendanceModelDB[]>> OrdersAttendancesByIssuesAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<RecordsAttendanceModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.OrdersAttendancesByIssuesGetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AttendanceRecordsDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.AttendanceRecordDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankDetailsUpdateAsync(TAuthRequestModel<BankDetailsModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.BankDetailsUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> BankDetailsDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.BankDetailsDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> IncomingMerchantPaymentTBankAsync(IncomingMerchantPaymentTBankNotifyModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.IncomingMerchantPaymentTBankReceive, req, false, token: token) ?? new();
}