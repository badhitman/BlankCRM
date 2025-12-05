////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsTransmission;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// Розница
/// </summary>
public class RetailTransmission(IRabbitClient rabbitClient) : IRetailService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRowRetailDocumentAsync(RowOfRetailOrderDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateRowDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRowRetailDocumentAsync(RowOfRetailOrderDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateRowDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RowOfRetailOrderDocumentModelDB>> SelectRowsRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RowOfRetailOrderDocumentModelDB>>(TransmissionQueues.SelectRowsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRetailDocumentAsync(RetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRetailDocumentAsync(RetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RetailDocumentModelDB>> SelectRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RetailDocumentModelDB>>(TransmissionQueues.SelectDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryServiceAsync(DeliveryServiceRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDeliveryServiceRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDeliveryStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreatePaymentDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentOrderLinkAsync(PaymentRetailOrderLinkModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreatePaymentOrderLinkRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateRowOfDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateWalletRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateWalletTypeRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DeliveryServiceRetailModelDB[]>> DeliveryServicesGetAsync(int[] reqIds, CancellationToken token = default)
      => await rabbitClient.MqRemoteCallAsync<TResponseModel<DeliveryServiceRetailModelDB[]>>(TransmissionQueues.DeliveryServicesGetRetailReceive, reqIds, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<DeliveryDocumentRetailModelDB>>(TransmissionQueues.SelectDeliveryDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryServiceRetailModelDB>> SelectDeliveryServicesAsync(TPaginationRequestStandardModel<SelectDeliveryServicesRetailRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<DeliveryServiceRetailModelDB>>(TransmissionQueues.SelectDeliveryServicesRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>>(TransmissionQueues.SelectDeliveryStatusesDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<PaymentRetailDocumentModelDB>>(TransmissionQueues.SelectPaymentsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailOrderLinkModelDB>> SelectPaymentsOrdersLinksAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersLinksRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<PaymentRetailOrderLinkModelDB>>(TransmissionQueues.SelectPaymentsOrdersLinksRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowsOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>>(TransmissionQueues.SelectRowsOfDeliveryDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WalletRetailModelDB>>(TransmissionQueues.SelectWalletsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailTypeViewModel>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WalletRetailTypeViewModel>>(TransmissionQueues.SelectWalletsTypesRetailReceive, req, token: token) ?? new();


    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryServiceAsync(DeliveryServiceRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDeliveryServiceRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDeliveryStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdatePaymentDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentOrderLinkAsync(PaymentRetailOrderLinkModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdatePaymentOrderLinkRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateRowOfDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateWalletRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateWalletTypeRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> WalletBalanceUpdateAsync(WalletBalanceCommitRequestModel req, CancellationToken token = default)
          => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.WalletBalanceUpdateRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WalletRetailTypeViewModel[]>> WalletsTypesGetAsync(int[] reqIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<WalletRetailTypeViewModel[]>>(TransmissionQueues.WalletsTypesGetRetailReceive, reqIds, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RetailDocumentModelDB[]>> RetailDocumentsGetAsync(RetailDocumentsGetRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<RetailDocumentModelDB[]>>(TransmissionQueues.DocumentsGetRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentRetailDeliveryLinkAsync(PaymentRetailDeliveryLinkModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreatePaymentDeliveryLinkRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentRetailDeliveryLinkAsync(PaymentRetailDeliveryLinkModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdatePaymentDeliveryLinkRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailDeliveryLinkModelDB>> SelectPaymentsRetailDeliveriesLinksAsync(TPaginationRequestStandardModel<SelectPaymentsRetailDeliveriesLinksRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<PaymentRetailDeliveryLinkModelDB>>(TransmissionQueues.SelectPaymentsDeliveriesLinksRetailReceive, req, token: token) ?? new();

    #region conversion`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateConversionDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateConversionDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>>(TransmissionQueues.SelectConversionsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WalletConversionRetailDocumentModelDB[]>> GetConversionsDocumentsAsync(ReadWalletsRetailsConversionDocumentsRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<WalletConversionRetailDocumentModelDB[]>>(TransmissionQueues.GetConversionsDocumentsRetailReceive, req, token: token) ?? new();
    #endregion
}