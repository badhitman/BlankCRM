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
    public async Task<TPaginationResponseModel<RecordsAttendanceModelDB>> RecordsAttendancesSelect(TPaginationRequestAuthModel<RecordsAttendancesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RecordsAttendanceModelDB>>(GlobalStaticConstants.TransmissionQueues.RecordsAttendancesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateAttendanceRecords(TAuthRequestModel<CreateAttendanceRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.CreateAttendanceRecordsCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusesOrdersAttendancesChangeByHelpdeskDocumentId(TAuthRequestModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.OrdersAttendancesStatusesChangeByHelpdeskDocumentIdReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> OrganizationOfferContractUpdate(TAuthRequestModel<OrganizationOfferToggleModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.OrganizationOfferContractUpdateOrCreateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<WorksFindResponseModel> WorksSchedulesFind(WorkFindRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<WorksFindResponseModel>(GlobalStaticConstants.TransmissionQueues.WorksSchedulesFindCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NomenclatureModelDB>> NomenclaturesSelect(TPaginationRequestModel<NomenclaturesSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<NomenclatureModelDB>>(GlobalStaticConstants.TransmissionQueues.NomenclaturesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OfficeOrganizationDelete(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.OfficeOrganizationDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfficeOrganizationUpdate(AddressOrganizationBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.OfficeOrganizationUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> NomenclatureUpdateReceive(NomenclatureModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.NomenclatureUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OfferDelete(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.OfferDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<OfferModelDB>>> OffersSelect(TAuthRequestModel<TPaginationRequestModel<OffersSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<OfferModelDB>>>(GlobalStaticConstants.TransmissionQueues.OfferSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OfferUpdate(TAuthRequestModel<OfferModelDB> offer, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.OfferUpdateCommerceReceive, offer, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>> OrdersRead(TAuthRequestModel<int[]> orders_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrderDocumentModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OrdersReadCommerceReceive, orders_ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> OrganizationSetLegal(OrganizationLegalModel org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.OrganizationSetLegalCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrganizationModelDB[]>> OrganizationsRead(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrganizationModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OrganizationsReadCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrganizationModelDB>> OrganizationsSelect(TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<OrganizationModelDB>>(GlobalStaticConstants.TransmissionQueues.OrganizationsSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OrganizationUpdate(TAuthRequestModel<OrganizationModelDB> org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.OrganizationUpdateOrCreateCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PaymentDocumentDelete(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.PaymentDocumentDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RowForOrderUpdate(RowOfOrderDocumentModelDB row, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.RowForOrderUpdateCommerceReceive, row, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> RowsForOrderDelete(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.RowsDeleteFromOrderCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OfficeOrganizationModelDB[]>> OfficesOrganizationsRead(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OfficeOrganizationModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OfficesOrganizationsReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<NomenclatureModelDB>>> NomenclaturesRead(TAuthRequestModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<NomenclatureModelDB>>>(GlobalStaticConstants.TransmissionQueues.NomenclaturesReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OfferModelDB[]>> OffersRead(TAuthRequestModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OfferModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OfferReadCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PriceRuleForOfferModelDB>>> PricesRulesGetForOffers(TAuthRequestModel<int[]> ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<PriceRuleForOfferModelDB>>>(GlobalStaticConstants.TransmissionQueues.PricesRulesGetForOfferCommerceReceive, ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrderDocumentModelDB>> OrdersSelect(TPaginationRequestModel<TAuthRequestModel<OrdersSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<OrderDocumentModelDB>>(GlobalStaticConstants.TransmissionQueues.OrdersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> OrderUpdate(OrderDocumentModelDB order, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.OrderUpdateCommerceReceive, order, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PaymentDocumentUpdate(TAuthRequestModel<PaymentDocumentBaseModel> payment, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.PaymentDocumentUpdateCommerceReceive, payment) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> PriceRuleUpdate(TAuthRequestModel<PriceRuleForOfferModelDB> price_rule, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.PriceRuleUpdateCommerceReceive, price_rule, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PriceRuleDelete(TAuthRequestModel<int> id, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.PriceRuleDeleteCommerceReceive, id, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusOrderChangeByHelpdeskDocumentId(TAuthRequestModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.StatusChangeOrderByHelpDeskDocumentIdReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>> OrdersByIssues(OrdersByIssuesSelectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<OrderDocumentModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OrdersByIssuesGetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FileAttachModel>> OrderReportGet(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FileAttachModel>>(GlobalStaticConstants.TransmissionQueues.OrderReportGetCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<FileAttachModel> PriceFullFileGet(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<FileAttachModel>(GlobalStaticConstants.TransmissionQueues.PriceFullFileGetCommerceReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> WeeklyScheduleUpdate(WeeklyScheduleModelDB work, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.WeeklyScheduleUpdateCommerceReceive, work, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WeeklyScheduleModelDB>> WeeklySchedulesSelect(TPaginationRequestModel<WorkSchedulesSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WeeklyScheduleModelDB>>(GlobalStaticConstants.TransmissionQueues.WeeklySchedulesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<WeeklyScheduleModelDB>> WeeklySchedulesRead(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<WeeklyScheduleModelDB>>(GlobalStaticConstants.TransmissionQueues.WeeklySchedulesReadCommerceReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CalendarScheduleUpdate(TAuthRequestModel<CalendarScheduleModelDB> work, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.CalendarScheduleUpdateCommerceReceive, work, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>> CalendarsSchedulesSelect(TAuthRequestModel<TPaginationRequestModel<WorkScheduleCalendarsSelectRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>>(GlobalStaticConstants.TransmissionQueues.CalendarsSchedulesSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CalendarScheduleModelDB>>> CalendarsSchedulesRead(TAuthRequestModel<int[]> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<CalendarScheduleModelDB>>>(GlobalStaticConstants.TransmissionQueues.CalendarsSchedulesReadCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>> UsersOrganizationsSelect(TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>>(GlobalStaticConstants.TransmissionQueues.OrganizationsUsersSelectCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> UserOrganizationUpdate(TAuthRequestModel<UserOrganizationModelDB> org, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.OrganizationUserUpdateOrCreateCommerceReceive, org, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserOrganizationModelDB[]>> UsersOrganizationsRead(int[] organizations_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserOrganizationModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OrganizationsUsersReadCommerceReceive, organizations_ids, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RecordsAttendanceModelDB[]>> OrdersAttendancesByIssues(OrdersByIssuesSelectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<RecordsAttendanceModelDB[]>>(GlobalStaticConstants.TransmissionQueues.OrdersAttendancesByIssuesGetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AttendanceRecordsDelete(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.AttendanceRecordDeleteCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankDetailsUpdate(TAuthRequestModel<BankDetailsModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.BankDetailsUpdateCommerceReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> BankDetailsDelete(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.BankDetailsDeleteCommerceReceive, req, token: token) ?? new();
}