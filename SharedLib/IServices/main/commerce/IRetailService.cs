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
    public Task<TResponseModel<int>> CreateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DeliveryDocumentRetailModelDB[]>> GetDeliveryDocumentsAsync(GetDeliveryDocumentsRetailRequestModel req, CancellationToken token = default);
    #endregion

    #region Row Of Delivery Document
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowsOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default);
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
    public Task<TResponseModel<int>> CreateDeliveryOrderLinkDocumentAsync(RetailDeliveryOrderLinkModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateDeliveryOrderLinkDocumentAsync(RetailDeliveryOrderLinkModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<RetailDeliveryOrderLinkModelDB>> SelectDeliveriesOrdersLinksDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default);

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
    public Task<TPaginationResponseModel<PaymentOrderRetailLinkModelDB>> SelectPaymentsOrdersDocumentsLinksAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeletePaymentOrderLinkDocumentAsync(DeletePaymentOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default);
    #endregion

    #region Payment Document
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<PaymentRetailDocumentModelDB[]>> GetPaymentsDocumentsAsync(GetPaymentsRetailOrdersDocumentsRequestModel req, CancellationToken token = default);
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
    public Task<TResponseModel<int>> CreateRetailDocumentAsync(DocumentRetailModelDB req, CancellationToken token = default);

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
    public Task<TResponseModel<int>> CreateRowRetailDocumentAsync(RowOfRetailOrderDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateRowRetailDocumentAsync(RowOfRetailOrderDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<RowOfRetailOrderDocumentModelDB>> SelectRowsRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel> req, CancellationToken token = default);
    #endregion

    #region Conversion document`s
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<WalletConversionRetailDocumentModelDB[]>> GetConversionsDocumentsAsync(ReadWalletsRetailsConversionDocumentsRequestModel req, CancellationToken token = default);
    #endregion
}