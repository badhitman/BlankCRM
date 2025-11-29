////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace CommerceService;

/// <summary>
/// Розница
/// </summary>
public class RetailService : IRetailService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryServiceAsync(DeliveryServiceRetailModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentOrderLinkAsync(PaymentRetailOrderLinkModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryServiceRetailModelDB>> SelectDeliveryServicesAsync(TPaginationRequestStandardModel<SelectDeliveryServicesRetailRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailOrderLinkModelDB>> SelectPaymentsOrdersLinksAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersLinksRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailTypeModelDB>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryServiceAsync(DeliveryServiceRetailModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentOrderLinkAsync(PaymentRetailOrderLinkModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
