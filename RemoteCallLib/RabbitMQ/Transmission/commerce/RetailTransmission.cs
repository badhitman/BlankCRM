////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DocumentFormat.OpenXml.Drawing;
using SharedLib;
using static SharedLib.GlobalStaticConstantsTransmission;

namespace RemoteCallLib;

/// <summary>
/// Розница
/// </summary>
public class RetailTransmission(IRabbitClient rabbitClient) : IRetailService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<DeliveryDocumentRetailModelDB[]>> GetDeliveryDocumentsAsync(GetDeliveryDocumentsRetailRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<DeliveryDocumentRetailModelDB[]>>(TransmissionQueues.GetDeliveryDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentRetailDocumentModelDB[]>> GetPaymentsDocumentsAsync(GetPaymentsRetailOrdersDocumentsRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<PaymentRetailDocumentModelDB[]>>(TransmissionQueues.GetPaymentsDocumentsRetailReceive, req, token: token) ?? new();

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
    public async Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDeliveryStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreatePaymentDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateRowOfDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateWalletRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<DeliveryDocumentRetailModelDB>>(TransmissionQueues.SelectDeliveryDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>>(TransmissionQueues.SelectDeliveryStatusesDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<PaymentRetailDocumentModelDB>>(TransmissionQueues.SelectPaymentsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowsOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>>(TransmissionQueues.SelectRowsOfDeliveryDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WalletRetailModelDB>>(TransmissionQueues.SelectWalletsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDeliveryStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdatePaymentDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateRowOfDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateWalletRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RetailDocumentModelDB[]>> RetailDocumentsGetAsync(RetailDocumentsGetRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<RetailDocumentModelDB[]>>(TransmissionQueues.DocumentsGetRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDeliveryStatusDocumentAsync(int statusId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteDeliveryStatusDocumentRetailReceive, statusId, token: token) ?? new();

    #region Wallet Type
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateWalletTypeRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateWalletTypeRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WalletRetailTypeViewModel[]>> WalletsTypesGetAsync(int[] reqIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<WalletRetailTypeViewModel[]>>(TransmissionQueues.WalletsTypesGetRetailReceive, reqIds, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailTypeViewModel>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WalletRetailTypeViewModel>>(TransmissionQueues.SelectWalletsTypesRetailReceive, req, token: token) ?? new();
    #endregion

    #region Deliveries orders link`s 
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryOrderLinkDocumentAsync(RetailDeliveryOrderLinkModelDB req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDeliveryOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<decimal>> TotalWeightOrdersLinksDocumentsAsync(TotalWeightDeliveriesOrdersLinksDocumentsRequestModel req, CancellationToken token = default)
      => await rabbitClient.MqRemoteCallAsync<TResponseModel<decimal>>(TransmissionQueues.TotalWeightOrdersLinksDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryOrderLinkDocumentAsync(RetailDeliveryOrderLinkModelDB req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDeliveryOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RetailDeliveryOrderLinkModelDB>> SelectDeliveriesOrdersLinksDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RetailDeliveryOrderLinkModelDB>>(TransmissionQueues.SelectDeliveriesOrdersLinksDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDeliveryOrderLinkDocumentAsync(DeleteDeliveryOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteDeliveryOrderLinkDocumentReceive, req, token: token) ?? new();
    #endregion

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