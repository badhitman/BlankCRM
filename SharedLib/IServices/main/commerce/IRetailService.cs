////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Розница
/// </summary>
public interface IRetailService
{
    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<WalletRetailTypeViewModel>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<WalletRetailTypeViewModel[]>> WalletsTypesGetAsync(int[] reqIds, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> WalletBalanceUpdateAsync(WalletBalanceCommitRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateWalletAsync(WalletRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateWalletAsync(WalletRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreatePaymentOrderLinkAsync(PaymentRetailOrderLinkModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdatePaymentOrderLinkAsync(PaymentRetailOrderLinkModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<PaymentRetailOrderLinkModelDB>> SelectPaymentsOrdersLinksAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersLinksRequestModel> req, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateDeliveryServiceAsync(DeliveryServiceRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateDeliveryServiceAsync(DeliveryServiceRetailModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<DeliveryServiceRetailModelDB>> SelectDeliveryServicesAsync(TPaginationRequestStandardModel<SelectDeliveryServicesRetailRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<DeliveryServiceRetailModelDB[]>> DeliveryServicesGetAsync(int[] reqIds, CancellationToken token = default);
}