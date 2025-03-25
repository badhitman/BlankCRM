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
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RecordsAttendanceModelDB>>(GlobalStaticConstants.TransmissionQueues.RecordsAttendancesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateAttendanceRecordsAsync(TAuthRequestModel<CreateAttendanceRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.CreateAttendanceRecordsCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusesOrdersAttendancesChangeByHelpdeskDocumentIdAsync(TAuthRequestModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.OrdersAttendancesStatusesChangeByHelpdeskDocumentIdReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> OrganizationOfferContractUpdateAsync(TAuthRequestModel<OrganizationOfferToggleModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.OrganizationOfferContractUpdateOrCreateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<WorksFindResponseModel> WorksSchedulesFindAsync(WorkFindRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<WorksFindResponseModel>(GlobalStaticConstants.TransmissionQueues.WorksSchedulesFindCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NomenclatureModelDB>> NomenclaturesSelectAsync(TPaginationRequestModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<NomenclatureModelDB>>(GlobalStaticConstants.TransmissionQueues.NomenclaturesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OfficeOrganizationDeleteAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.OfficeOrganizationDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfficeOrganizationUpdateAsync(AddressOrganizationBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.OfficeOrganizationUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> NomenclatureUpdateAsync(NomenclatureModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.NomenclatureUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OfferDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.OfferDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<OfferModelDB>>> OffersSelectAsync(TAuthRequestModel<TPaginationRequestModel<OffersSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<OfferModelDB>>>(GlobalStaticConstants.TransmissionQueues.OfferSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfferUpdateAsync(TAuthRequestModel<OfferModelDB> offer, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.OfferUpdateCommerceReceive, offer, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>> OrdersReadAsync(TAuthRequestModel<int[]> orders_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrderDocumentModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OrdersReadCommerceReceive, orders_ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> OrganizationSetLegalAsync(OrganizationLegalModel org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.OrganizationSetLegalCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrganizationModelDB[]>> OrganizationsReadAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrganizationModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OrganizationsReadCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrganizationModelDB>> OrganizationsSelectAsync(TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<OrganizationModelDB>>(GlobalStaticConstants.TransmissionQueues.OrganizationsSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OrganizationUpdateAsync(TAuthRequestModel<OrganizationModelDB> org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.OrganizationUpdateOrCreateCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PaymentDocumentDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.PaymentDocumentDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RowForOrderUpdateAsync(RowOfOrderDocumentModelDB row, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.RowForOrderUpdateCommerceReceive, row, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> RowsForOrderDeleteAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.RowsDeleteFromOrderCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OfficeOrganizationModelDB[]>> OfficesOrganizationsReadAsync(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OfficeOrganizationModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OfficesOrganizationsReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesReadAsync(TAuthRequestModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<NomenclatureModelDB>>>(GlobalStaticConstants.TransmissionQueues.NomenclaturesReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OfferModelDB[]>> OffersReadAsync(TAuthRequestModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OfferModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OfferReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffersAsync(TAuthRequestModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<PriceRuleForOfferModelDB>>>(GlobalStaticConstants.TransmissionQueues.PricesRulesGetForOfferCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrderDocumentModelDB>> OrdersSelectAsync(TPaginationRequestModel<TAuthRequestModel<OrdersSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<OrderDocumentModelDB>>(GlobalStaticConstants.TransmissionQueues.OrdersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OrderUpdateAsync(OrderDocumentModelDB order, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.OrderUpdateCommerceReceive, order, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PaymentDocumentUpdateAsync(TAuthRequestModel<PaymentDocumentBaseModel> payment, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.PaymentDocumentUpdateCommerceReceive, payment) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PriceRuleUpdateAsync(TAuthRequestModel<PriceRuleForOfferModelDB> price_rule, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.PriceRuleUpdateCommerceReceive, price_rule, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PriceRuleDeleteAsync(TAuthRequestModel<int> id, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.PriceRuleDeleteCommerceReceive, id, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusOrderChangeByHelpdeskDocumentIdAsync(TAuthRequestModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.StatusChangeOrderByHelpDeskDocumentIdReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssuesAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrderDocumentModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OrdersByIssuesGetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FileAttachModel>> OrderReportGetAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FileAttachModel>>(GlobalStaticConstants.TransmissionQueues.OrderReportGetCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<FileAttachModel> PriceFullFileGetAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<FileAttachModel>(GlobalStaticConstants.TransmissionQueues.PriceFullFileGetCommerceReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WeeklyScheduleUpdateAsync(WeeklyScheduleModelDB work, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.WeeklyScheduleUpdateCommerceReceive, work, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WeeklyScheduleModelDB>> WeeklySchedulesSelectAsync(TPaginationRequestModel<WorkSchedulesSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WeeklyScheduleModelDB>>(GlobalStaticConstants.TransmissionQueues.WeeklySchedulesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<WeeklyScheduleModelDB>> WeeklySchedulesReadAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<WeeklyScheduleModelDB>>(GlobalStaticConstants.TransmissionQueues.WeeklySchedulesReadCommerceReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CalendarScheduleUpdateAsync(TAuthRequestModel<CalendarScheduleModelDB> work, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.CalendarScheduleUpdateCommerceReceive, work, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>> CalendarsSchedulesSelectAsync(TAuthRequestModel<TPaginationRequestModel<WorkScheduleCalendarsSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>>(GlobalStaticConstants.TransmissionQueues.CalendarsSchedulesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CalendarScheduleModelDB>>> CalendarsSchedulesReadAsync(TAuthRequestModel<int[]> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<CalendarScheduleModelDB>>>(GlobalStaticConstants.TransmissionQueues.CalendarsSchedulesReadCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>> UsersOrganizationsSelectAsync(TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>>(GlobalStaticConstants.TransmissionQueues.OrganizationsUsersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> UserOrganizationUpdateAsync(TAuthRequestModel<UserOrganizationModelDB> org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.OrganizationUserUpdateOrCreateCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserOrganizationModelDB[]>> UsersOrganizationsReadAsync(int[] organizations_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserOrganizationModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OrganizationsUsersReadCommerceReceive, organizations_ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RecordsAttendanceModelDB[]>> OrdersAttendancesByIssuesAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<RecordsAttendanceModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OrdersAttendancesByIssuesGetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AttendanceRecordsDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.AttendanceRecordDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankDetailsUpdateAsync(TAuthRequestModel<BankDetailsModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.BankDetailsUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> BankDetailsDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.BankDetailsDeleteCommerceReceive, req, token: token) ?? new();
}