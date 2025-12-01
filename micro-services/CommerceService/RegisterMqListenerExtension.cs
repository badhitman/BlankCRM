////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Transmission.Receives.commerce;
using SharedLib;

namespace CommerceService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection CommerceRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<UpdateWalletTypeReceive, WalletRetailTypeModelDB, ResponseBaseModel>()
            .RegisterMqListener<UpdateWalletReceive, WalletRetailModelDB, ResponseBaseModel>()
            .RegisterMqListener<UpdateRowOfDeliveryDocumentReceive, RowOfDeliveryRetailDocumentModelDB, ResponseBaseModel>()
            .RegisterMqListener<UpdatePaymentOrderLinkReceive, PaymentRetailOrderLinkModelDB, ResponseBaseModel>()
            .RegisterMqListener<UpdatePaymentDocumentReceive, PaymentRetailDocumentModelDB, ResponseBaseModel>()
            .RegisterMqListener<UpdateDeliveryStatusDocumentReceive, DeliveryStatusRetailDocumentModelDB, ResponseBaseModel>()
            .RegisterMqListener<UpdateDeliveryServiceReceive, DeliveryServiceRetailModelDB, ResponseBaseModel>()
            .RegisterMqListener<UpdateDeliveryDocumentReceive, DeliveryDocumentRetailModelDB, ResponseBaseModel>()
            .RegisterMqListener<UpdateConversionDocumentReceive, WalletConversionRetailDocumentModelDB, ResponseBaseModel>()
            .RegisterMqListener<SelectWalletsTypesReceive, TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel>, TPaginationResponseModel<WalletRetailTypeViewModel>>()
            .RegisterMqListener<SelectWalletsReceive, TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel>, TPaginationResponseModel<WalletRetailModelDB>>()
            .RegisterMqListener<SelectRowOfDeliveryDocumentsReceive, TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel>, TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>>()
            .RegisterMqListener<SelectPaymentsOrdersLinksReceive, TPaginationRequestStandardModel<SelectPaymentsRetailOrdersLinksRequestModel>, TPaginationResponseModel<PaymentRetailOrderLinkModelDB>>()
            .RegisterMqListener<SelectPaymentsDocumentsReceive, TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel>, TPaginationResponseModel<PaymentRetailDocumentModelDB>>()
            .RegisterMqListener<SelectDeliveryStatusesDocumentsReceive, TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel>, TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>>()
            .RegisterMqListener<SelectDeliveryServicesReceive, TPaginationRequestStandardModel<SelectDeliveryServicesRetailRequestModel>, TPaginationResponseModel<DeliveryServiceRetailModelDB>>()
            .RegisterMqListener<SelectDeliveryDocumentsReceive, TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel>, TPaginationResponseModel<DeliveryDocumentRetailModelDB>>()
            .RegisterMqListener<SelectConversionsDocumentsReceive, TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel>, TPaginationResponseModel<WalletConversionRetailDocumentModelDB>>()
            .RegisterMqListener<CreateWalletTypeReceive, WalletRetailTypeModelDB, TResponseModel<int>>()
            .RegisterMqListener<CreateWalletReceive, WalletRetailModelDB, TResponseModel<int>>()
            .RegisterMqListener<CreateRowOfDeliveryDocumentReceive, RowOfDeliveryRetailDocumentModelDB, TResponseModel<int>>()
            .RegisterMqListener<CreatePaymentOrderLinkReceive, PaymentRetailOrderLinkModelDB, TResponseModel<int>>()
            .RegisterMqListener<CreatePaymentDocumentReceive, PaymentRetailDocumentModelDB, TResponseModel<int>>()
            .RegisterMqListener<CreateDeliveryStatusDocumentReceive, DeliveryStatusRetailDocumentModelDB, TResponseModel<int>>()
            .RegisterMqListener<CreateDeliveryServiceReceive, DeliveryServiceRetailModelDB, TResponseModel<int>>()
            .RegisterMqListener<CreateDeliveryDocumentReceive, DeliveryDocumentRetailModelDB, TResponseModel<int>>()
            .RegisterMqListener<CreateConversionDocumentReceive, WalletConversionRetailDocumentModelDB, TResponseModel<int>>()
            .RegisterMqListener<WalletBalanceUpdateReceive, WalletBalanceCommitRequestModel, ResponseBaseModel>()
            .RegisterMqListener<WalletsTypesGetReceive, int[], TResponseModel<WalletRetailTypeViewModel[]>>()
            .RegisterMqListener<DeliveryServicesGetReceive, int[], TResponseModel<DeliveryServiceRetailModelDB[]>>()
            .RegisterMqListener<SelectRetailDocumentsReceive, TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel>, TPaginationResponseModel<RetailDocumentModelDB>>()
            .RegisterMqListener<CreateRetailDocumentReceive, RetailDocumentModelDB, TResponseModel<int>>()
            .RegisterMqListener<UpdateRetailDocumentReceive, RetailDocumentModelDB, ResponseBaseModel>()
            .RegisterMqListener<RetailDocumentsGetReceive, RetailDocumentsGetRequestModel, TResponseModel<RetailDocumentModelDB[]>>()

            .RegisterMqListener<OrganizationSetLegalReceive, OrganizationLegalModel, TResponseModel<bool>>()
            .RegisterMqListener<OrganizationUpdateReceive, TAuthRequestModel<OrganizationModelDB>, TResponseModel<int>>()
            .RegisterMqListener<OrganizationsSelectReceive, TPaginationRequestAuthModel<OrganizationsSelectRequestModel>, TPaginationResponseModel<OrganizationModelDB>>()
            .RegisterMqListener<OfficeOrganizationUpdateReceive, AddressOrganizationBaseModel, TResponseModel<int>>()
            .RegisterMqListener<OfficesOrganizationDeleteReceive, int, ResponseBaseModel>()
            .RegisterMqListener<RecordsAttendancesSelectReceive, TPaginationRequestAuthModel<RecordsAttendancesRequestModel>, TPaginationResponseModel<RecordsAttendanceModelDB>>()
            .RegisterMqListener<NomenclatureUpdateReceive, NomenclatureModelDB, TResponseModel<int>>()
            .RegisterMqListener<OrdersByIssuesGetReceive, OrdersByIssuesSelectRequestModel, TResponseModel<OrderDocumentModelDB[]>>()
            .RegisterMqListener<OfferDeleteReceive, TAuthRequestModel<int>, ResponseBaseModel>()
            .RegisterMqListener<AttendanceRecordsDeleteReceive, TAuthRequestModel<int>, ResponseBaseModel>()
            .RegisterMqListener<UserOrganizationUpdateReceive, TAuthRequestModel<UserOrganizationModelDB>, TResponseModel<int>>()
            .RegisterMqListener<UsersOrganizationsReadReceive, int[], TResponseModel<UserOrganizationModelDB[]>>()
            .RegisterMqListener<UsersOrganizationsSelectReceive, TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel>, TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>>()
            .RegisterMqListener<BankDetailsDeleteReceive, TAuthRequestModel<int>, ResponseBaseModel>()
            .RegisterMqListener<BankDetailsUpdateReceive, TAuthRequestModel<BankDetailsModelDB>, TResponseModel<int>>()
            .RegisterMqListener<WeeklyScheduleUpdateReceive, WeeklyScheduleModelDB, TResponseModel<int>>()
            .RegisterMqListener<WeeklySchedulesSelectReceive, TPaginationRequestStandardModel<WorkSchedulesSelectRequestModel>, TPaginationResponseModel<WeeklyScheduleModelDB>>()
            .RegisterMqListener<WeeklySchedulesReadReceive, int[], List<WeeklyScheduleModelDB>>()
            .RegisterMqListener<CalendarScheduleUpdateReceive, TAuthRequestModel<CalendarScheduleModelDB>, TResponseModel<int>>()
            .RegisterMqListener<CalendarsSchedulesSelectReceive, TAuthRequestModel<TPaginationRequestStandardModel<WorkScheduleCalendarsSelectRequestModel>>, TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>>()
            .RegisterMqListener<CalendarsSchedulesReadReceive, TAuthRequestModel<int[]>, TResponseModel<List<CalendarScheduleModelDB>>>()
            .RegisterMqListener<PriceFullFileGetReceive, object, FileAttachModel>()
            .RegisterMqListener<OrderReportGetReceive, TAuthRequestModel<int>, TResponseModel<FileAttachModel>>()
            .RegisterMqListener<OffersRegistersSelectReceive, TPaginationRequestStandardModel<RegistersSelectRequestBaseModel>, TPaginationResponseModel<OfferAvailabilityModelDB>>()
            .RegisterMqListener<WarehousesSelectReceive, TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel>, TPaginationResponseModel<WarehouseDocumentModelDB>>()
            .RegisterMqListener<WarehousesDocumentsReadReceive, int[], TResponseModel<WarehouseDocumentModelDB[]>>()
            .RegisterMqListener<WarehouseDocumentUpdateReceive, WarehouseDocumentModelDB, TResponseModel<int>>()
            .RegisterMqListener<RowsForWarehouseDocumentDeleteReceive, int[], TResponseModel<bool>>()
            .RegisterMqListener<RowForWarehouseDocumentUpdateReceive, RowOfWarehouseDocumentModelDB, TResponseModel<int>>()
            .RegisterMqListener<StatusOrderChangeByHelpDeskDocumentIdReceive, TAuthRequestModel<StatusChangeRequestModel>, TResponseModel<bool>>()
            .RegisterMqListener<PriceRuleDeleteReceive, TAuthRequestModel<int>, ResponseBaseModel>()
            .RegisterMqListener<AttendancesRecordsByIssuesGetReceive, OrdersByIssuesSelectRequestModel, TResponseModel<RecordsAttendanceModelDB[]>>()
            .RegisterMqListener<WorksFindReceive, WorkFindRequestModel, WorksFindResponseModel>()
            .RegisterMqListener<PriceRuleUpdateReceive, TAuthRequestModel<PriceRuleForOfferModelDB>, TResponseModel<int>>()
            .RegisterMqListener<PricesRulesGetForOffersReceive, TAuthRequestModel<int[]>, TResponseModel<List<PriceRuleForOfferModelDB>>>()
            .RegisterMqListener<PaymentDocumentUpdateReceive, TAuthRequestModel<PaymentDocumentBaseModel>, TResponseModel<int>>()
            .RegisterMqListener<OrderUpdateReceive, OrderDocumentModelDB, TResponseModel<int>>()
            .RegisterMqListener<OffersReadReceive, TAuthRequestModel<int[]>, TResponseModel<OfferModelDB[]>>()
            .RegisterMqListener<NomenclaturesReadReceive, TAuthRequestModel<int[]>, TResponseModel<List<NomenclatureModelDB>>>()
            .RegisterMqListener<OfficesOrganizationsReadReceive, int[], TResponseModel<OfficeOrganizationModelDB[]>>()
            .RegisterMqListener<PaymentDocumentDeleteReceive, TAuthRequestModel<int>, ResponseBaseModel>()
            .RegisterMqListener<RowsForOrderDeleteReceive, int[], TResponseModel<bool>>()
            .RegisterMqListener<AttendanceRecordsCreateReceive, TAuthRequestModel<CreateAttendanceRequestModel>, ResponseBaseModel>()
            .RegisterMqListener<RowForOrderUpdateReceive, RowOfOrderDocumentModelDB, TResponseModel<int>>()
            .RegisterMqListener<OrdersReadReceive, TAuthRequestModel<int[]>, TResponseModel<OrderDocumentModelDB[]>>()
            .RegisterMqListener<OrganizationOfferContractUpdateReceive, TAuthRequestModel<OrganizationOfferToggleModel>, TResponseModel<bool>>()
            .RegisterMqListener<OrdersSelectReceive, TPaginationRequestStandardModel<TAuthRequestModel<OrdersSelectRequestModel>>, TPaginationResponseModel<OrderDocumentModelDB>>()
            .RegisterMqListener<OfferUpdateReceive, TAuthRequestModel<OfferModelDB>, TResponseModel<int>>()
            .RegisterMqListener<OffersSelectReceive, TAuthRequestModel<TPaginationRequestStandardModel<OffersSelectRequestModel>>, TResponseModel<TPaginationResponseModel<OfferModelDB>>>()
            .RegisterMqListener<NomenclaturesSelectReceive, TPaginationRequestStandardModel<NomenclaturesSelectRequestModel>, TPaginationResponseModel<NomenclatureModelDB>>()
            .RegisterMqListener<OrganizationsReadReceive, int[], TResponseModel<OrganizationModelDB[]>>()
            .RegisterMqListener<IncomingMerchantPaymentTBankReceive, IncomingMerchantPaymentTBankNotifyModel, ResponseBaseModel>()
        ;
    }
}