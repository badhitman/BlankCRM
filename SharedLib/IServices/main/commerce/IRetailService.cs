////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Розница
/// </summary>
public interface IRetailService
{
    #region Delivery Document
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateDeliveryDocumentAsync(CreateDeliveryDocumentRetailRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DeliveryDocumentRetailModelDB[]>> GetDeliveryDocumentsAsync(GetDeliveryDocumentsRetailRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<FileAttachModel> GetDeliveriesJournalFileAsync(SelectDeliveryDocumentsRetailRequestModel req, CancellationToken token = default);
    #endregion

    #region Row Of Delivery Document
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowsOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteRowOfDeliveryDocumentAsync(int rowId, CancellationToken token = default);
    #endregion

    #region Statuses (of delivery document)
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteDeliveryStatusDocumentAsync(int statusId, CancellationToken token = default);
    #endregion

    #region Deliveries orders link`s    
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateDeliveryOrderLinkDocumentAsync(RetailOrderDeliveryLinkModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateDeliveryOrderLinkDocumentAsync(RetailOrderDeliveryLinkModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<RetailOrderDeliveryLinkModelDB>> SelectDeliveriesOrdersLinksDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<decimal>> TotalWeightOrdersDocumentsLinksAsync(TotalWeightDeliveriesOrdersLinksDocumentsRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteDeliveryOrderLinkDocumentAsync(DeleteDeliveryOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default);
    #endregion

    #region Payments orders link`s
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreatePaymentOrderLinkDocumentAsync(PaymentOrderRetailLinkModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdatePaymentOrderLinkDocumentAsync(PaymentOrderRetailLinkModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<PaymentOrderRetailLinkModelDB>> SelectPaymentsOrdersDocumentsLinksAsync(TPaginationRequestStandardModel<SelectPaymentsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeletePaymentOrderLinkDocumentAsync(DeletePaymentOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default);
    #endregion

    #region Payment Document
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreatePaymentDocumentAsync(CreatePaymentRetailDocumentRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<PaymentRetailDocumentModelDB[]>> GetPaymentsDocumentsAsync(GetPaymentsRetailOrdersDocumentsRequestModel req, CancellationToken token = default);
    #endregion

    #region Report`s
    /// <inheritdoc/>
    public Task<TPaginationResponseModel<WalletRetailReportRowModel>> FinancialsReportRetailAsync(TPaginationRequestStandardModel<SelectPaymentsRetailReportRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<OffersRetailReportRowModel>> OffersOfOrdersReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfOrdersRetailReportRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<OffersRetailReportRowModel>> OffersOfDeliveriesReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<MainReportResponseModel> GetMainReportAsync(MainReportRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<PeriodBaseModel> AboutPeriodAsync(object? req = default, CancellationToken token = default);
    #endregion

    #region Wallet Type
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<WalletRetailTypeViewModel>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<WalletRetailTypeViewModel[]>> WalletsTypesGetAsync(int[] reqIds, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> ToggleWalletTypeDisabledForPaymentTypeAsync(ToggleWalletTypeDisabledForPaymentTypeRequestModel req, CancellationToken token = default);
    #endregion

    #region Wallet`s
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateWalletAsync(WalletRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateWalletAsync(WalletRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default);
    #endregion

    #region Order`s (document retail)
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateRetailDocumentAsync(CreateDocumentRetailRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateRetailDocumentAsync(DocumentRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<DocumentRetailModelDB>> SelectRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DocumentRetailModelDB[]>> RetailDocumentsGetAsync(RetailDocumentsGetRequestModel req, CancellationToken token = default);
    #endregion

    #region Statuses (of order`s document)
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateOrderStatusDocumentAsync(OrderStatusRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateOrderStatusDocumentAsync(OrderStatusRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<OrderStatusRetailDocumentModelDB>> SelectOrderDocumentStatusesAsync(TPaginationRequestStandardModel<SelectOrderStatusesRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteOrderStatusDocumentAsync(int statusId, CancellationToken token = default);
    #endregion

    #region Rows for retail order (document)
    /// <inheritdoc/>
    public Task<TResponseModel<KeyValuePair<int, Guid>?>> CreateRowRetailDocumentAsync(RowOfRetailOrderDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<Guid?>> UpdateRowRetailDocumentAsync(RowOfRetailOrderDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<Guid?>> DeleteRowRetailDocumentAsync(DeleteRowRetailDocumentRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<RowOfRetailOrderDocumentModelDB>> SelectRowsRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel> req, CancellationToken token = default);
    #endregion

    #region Conversion document`s
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateConversionDocumentRetailAsync(CreateWalletConversionRetailDocumentRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateConversionDocumentRetailAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsRetailAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<WalletConversionRetailDocumentModelDB[]>> GetConversionsDocumentsRetailAsync(ReadWalletsRetailsConversionDocumentsRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteToggleConversionRetailAsync(int conversionId, CancellationToken token = default);
    #endregion

    #region Conversions orders link`s
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateConversionOrderLinkDocumentRetailAsync(ConversionOrderRetailLinkModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateConversionOrderLinkDocumentRetailAsync(ConversionOrderRetailLinkModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<ConversionOrderRetailLinkModelDB>> SelectConversionsOrdersDocumentsLinksRetailAsync(TPaginationRequestStandardModel<SelectConversionsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteConversionOrderLinkDocumentRetailAsync(DeleteConversionOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default);
    #endregion
}