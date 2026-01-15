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
    public Task<TResponseModel<int>> CreateDeliveryDocumentAsync(TAuthRequestStandardModel<CreateDeliveryDocumentRetailRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<Guid?>> UpdateDeliveryDocumentAsync(TAuthRequestStandardModel<DeliveryDocumentRetailModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DeliveryDocumentRetailModelDB[]>> GetDeliveryDocumentsAsync(GetDeliveryDocumentsRetailRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<FileAttachModel> GetDeliveriesJournalFileAsync(SelectDeliveryDocumentsRetailRequestModel req, CancellationToken token = default);
    #endregion

    #region Row Of Delivery Document
    /// <inheritdoc/>
    public Task<CreateRowOfDeliveryDocumentResponseModel> CreateRowOfDeliveryDocumentAsync(TAuthRequestStandardModel<RowOfDeliveryRetailDocumentModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DeliveryDocumentRetailModelDB>> UpdateRowOfDeliveryDocumentAsync(TAuthRequestStandardModel<RowOfDeliveryRetailDocumentModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowsOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<RowOfDeliveryRetailDocumentModelDB>> DeleteRowOfDeliveryDocumentAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
    #endregion

    #region Statuses (of delivery document)
    /// <inheritdoc/>
    public Task<CreateDeliveryStatusDocumentResponseModel> CreateDeliveryStatusDocumentAsync(TAuthRequestStandardModel<DeliveryStatusRetailDocumentModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DeliveryDocumentRetailModelDB>> UpdateDeliveryStatusDocumentAsync(TAuthRequestStandardModel<DeliveryStatusRetailDocumentModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<DeleteDeliveryStatusDocumentResponseModel> DeleteDeliveryStatusDocumentAsync(TAuthRequestStandardModel<int> statusId, CancellationToken token = default);
    #endregion

    #region Deliveries orders link`s    
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateDeliveryOrderLinkDocumentAsync(TAuthRequestStandardModel<RetailOrderDeliveryLinkModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateDeliveryOrderLinkDocumentAsync(TAuthRequestStandardModel<RetailOrderDeliveryLinkModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<RetailOrderDeliveryLinkModelDB>> SelectDeliveriesOrdersLinksDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<RetailOrderDeliveryLinkModelDB[]>> DeliveriesOrdersLinksDocumentsReadAsync(int[] req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<decimal>> TotalWeightOrdersDocumentsLinksAsync(TotalWeightDeliveriesOrdersLinksDocumentsRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteDeliveryOrderLinkDocumentAsync(TAuthRequestStandardModel<OrderDeliveryModel> req, CancellationToken token = default);
    #endregion

    #region Payments orders link`s
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreatePaymentOrderLinkDocumentAsync(TAuthRequestStandardModel<PaymentOrderRetailLinkModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdatePaymentOrderLinkDocumentAsync(TAuthRequestStandardModel<PaymentOrderRetailLinkModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<PaymentOrderRetailLinkModelDB>> SelectPaymentsOrdersDocumentsLinksAsync(TPaginationRequestStandardModel<SelectPaymentsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<PaymentOrderRetailLinkModelDB[]>> PaymentsOrdersDocumentsLinksGetAsync(int[] req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeletePaymentOrderLinkDocumentAsync(TAuthRequestStandardModel<OrderPaymentModel> req, CancellationToken token = default);
    #endregion

    #region Payment Document
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreatePaymentDocumentAsync(TAuthRequestStandardModel<CreatePaymentRetailDocumentRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<Guid?>> UpdatePaymentDocumentAsync(TAuthRequestStandardModel<PaymentRetailDocumentModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<PaymentRetailDocumentModelDB[]>> GetPaymentsDocumentsAsync(GetPaymentsRetailOrdersDocumentsRequestModel req, CancellationToken token = default);
    #endregion

    #region Report`s
    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<WalletRetailReportRowModel>> FinancialsReportRetailAsync(TPaginationRequestStandardModel<SelectPaymentsRetailReportRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<OffersRetailReportRowModel>> OffersOfOrdersReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfOrdersRetailReportRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<OffersRetailReportRowModel>> OffersOfDeliveriesReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<MainReportResponseModel> GetMainReportAsync(MainReportRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<PeriodBaseModel> AboutPeriodAsync(object? req = default, CancellationToken token = default);
    #endregion

    #region Wallet Type
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateWalletTypeAsync(TAuthRequestStandardModel<WalletRetailTypeModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateWalletTypeAsync(TAuthRequestStandardModel<WalletRetailTypeModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<WalletRetailTypeViewModel>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<WalletRetailTypeViewModel[]>> WalletsTypesGetAsync(int[] reqIds, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> ToggleWalletTypeDisabledForPaymentTypeAsync(TAuthRequestStandardModel<ToggleWalletTypeDisabledForPaymentTypeRequestModel> req, CancellationToken token = default);
    #endregion

    #region Wallet`s
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateWalletAsync(WalletRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateWalletAsync(WalletRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default);
    #endregion

    #region Order`s (document retail)
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateRetailDocumentAsync(TAuthRequestStandardModel<CreateDocumentRetailRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<Guid>> UpdateRetailDocumentAsync(TAuthRequestStandardModel<DocumentRetailModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<DocumentRetailModelDB>> SelectRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DocumentRetailModelDB[]>> RetailDocumentsGetAsync(RetailDocumentsGetRequestModel req, CancellationToken token = default);
    #endregion

    #region Statuses (of order`s document)
    /// <inheritdoc/>
    public Task<TResponseModel<KeyValuePair<int, DocumentRetailModelDB>>> CreateOrderStatusDocumentAsync(TAuthRequestStandardModel<OrderStatusRetailDocumentModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DocumentRetailModelDB>> UpdateOrderStatusDocumentAsync(TAuthRequestStandardModel<OrderStatusRetailDocumentModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<OrderStatusRetailDocumentModelDB>> SelectOrderDocumentStatusesAsync(TPaginationRequestStandardModel<SelectOrderStatusesRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DocumentRetailModelDB>> DeleteOrderStatusDocumentAsync(TAuthRequestStandardModel<int> statusId, CancellationToken token = default);
    #endregion

    #region Rows for retail order (document)
    /// <inheritdoc/>
    public Task<TResponseModel<KeyValuePair<int, DocumentRetailModelDB>?>> CreateRowRetailDocumentAsync(TAuthRequestStandardModel<RowOfRetailOrderDocumentModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DocumentRetailModelDB>> UpdateRowRetailDocumentAsync(TAuthRequestStandardModel<RowOfRetailOrderDocumentModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<RowOfRetailOrderDocumentModelDB?>> DeleteRowRetailDocumentAsync(TAuthRequestStandardModel<DeleteRowRetailDocumentRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<RowOfRetailOrderDocumentModelDB>> SelectRowsRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel> req, CancellationToken token = default);
    #endregion

    #region Conversion document`s
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateConversionDocumentRetailAsync(TAuthRequestStandardModel<CreateWalletConversionRetailDocumentRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateConversionDocumentRetailAsync(TAuthRequestStandardModel<WalletConversionRetailDocumentModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsRetailAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<WalletConversionRetailDocumentModelDB[]>> GetConversionsDocumentsRetailAsync(ReadWalletsRetailsConversionDocumentsRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<WalletConversionRetailDocumentModelDB>> DeleteToggleConversionRetailAsync(TAuthRequestStandardModel<int> conversionId, CancellationToken token = default);
    #endregion

    #region Conversions orders link`s
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateConversionOrderLinkDocumentRetailAsync(TAuthRequestStandardModel<OrderConversionAmountModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateConversionOrderLinkDocumentRetailAsync(TAuthRequestStandardModel<OrderConversionAmountModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<ConversionOrderRetailLinkModelDB>> SelectConversionsOrdersDocumentsLinksRetailAsync(TPaginationRequestStandardModel<SelectConversionsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<ConversionOrderRetailLinkModelDB[]>> ConversionsOrdersDocumentsLinksReadRetailAsync(int[] req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteConversionOrderLinkDocumentRetailAsync(TAuthRequestStandardModel<OrderConversionModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<decimal>> GetSumConversionsOrdersAmountsAsync(GetSumConversionsOrdersAmountsRequestModel req, CancellationToken token = default);
    #endregion
}