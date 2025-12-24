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
    #region Order`s (document retail)
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRetailDocumentAsync(CreateDocumentRetailRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRetailDocumentAsync(DocumentRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentRetailModelDB[]>> RetailDocumentsGetAsync(RetailDocumentsGetRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentRetailModelDB[]>>(TransmissionQueues.DocumentsGetRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DocumentRetailModelDB>> SelectRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<DocumentRetailModelDB>>(TransmissionQueues.SelectDocumentsRetailReceive, req, token: token) ?? new();
    #endregion

    #region Rows for retail order (document)
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
    public async Task<ResponseBaseModel> DeleteRowRetailDocumentAsync(int rowId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteRowDocumentRetailReceive, rowId, token: token) ?? new();
    #endregion

    #region Statuses (of order`s document)
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateOrderStatusDocumentAsync(OrderStatusRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateOrderStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateOrderStatusDocumentAsync(OrderStatusRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateOrderStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrderStatusRetailDocumentModelDB>> SelectOrderDocumentStatusesAsync(TPaginationRequestStandardModel<SelectOrderStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<OrderStatusRetailDocumentModelDB>>(TransmissionQueues.SelectOrderDocumentStatusesRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteOrderStatusDocumentAsync(int statusId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteOrderStatusDocumentRetailReceive, statusId, token: token) ?? new();
    #endregion

    #region Delivery Document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryDocumentAsync(CreateDeliveryDocumentRetailRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<DeliveryDocumentRetailModelDB>>(TransmissionQueues.SelectDeliveryDocumentsRetailReceive, req, token: token) ?? new();
    /// <inheritdoc/>
    public async Task<TResponseModel<DeliveryDocumentRetailModelDB[]>> GetDeliveryDocumentsAsync(GetDeliveryDocumentsRetailRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<DeliveryDocumentRetailModelDB[]>>(TransmissionQueues.GetDeliveryDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<FileAttachModel> GetDeliveriesJournalFileAsync(SelectDeliveryDocumentsRetailRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<FileAttachModel>(TransmissionQueues.GetDeliveriesJournalFileRetailReceive, req, token: token) ?? new();
    #endregion

    #region Row Of Delivery Document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateRowOfDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateRowOfDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowsOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>>(TransmissionQueues.SelectRowsOfDeliveryDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRowOfDeliveryDocumentAsync(int rowId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteRowOfDeliveryDocumentRetailReceive, rowId, token: token) ?? new();
    #endregion

    #region Statuses (of delivery document)
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDeliveryStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDeliveryStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>>(TransmissionQueues.SelectDeliveryStatusesDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDeliveryStatusDocumentAsync(int statusId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteDeliveryStatusDocumentRetailReceive, statusId, token: token) ?? new();
    #endregion

    #region Deliveries orders link`s 
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryOrderLinkDocumentAsync(RetailOrderDeliveryLinkModelDB req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDeliveryOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<decimal>> TotalWeightOrdersDocumentsLinksAsync(TotalWeightDeliveriesOrdersLinksDocumentsRequestModel req, CancellationToken token = default)
      => await rabbitClient.MqRemoteCallAsync<TResponseModel<decimal>>(TransmissionQueues.TotalWeightOrdersLinksDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryOrderLinkDocumentAsync(RetailOrderDeliveryLinkModelDB req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDeliveryOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RetailOrderDeliveryLinkModelDB>> SelectDeliveriesOrdersLinksDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<RetailOrderDeliveryLinkModelDB>>(TransmissionQueues.SelectDeliveriesOrdersLinksDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDeliveryOrderLinkDocumentAsync(DeleteDeliveryOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteDeliveryOrderLinkDocumentReceive, req, token: token) ?? new();
    #endregion

    #region Payment Document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentDocumentAsync(CreatePaymentRetailDocumentRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreatePaymentDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdatePaymentDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<PaymentRetailDocumentModelDB>>(TransmissionQueues.SelectPaymentsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentRetailDocumentModelDB[]>> GetPaymentsDocumentsAsync(GetPaymentsRetailOrdersDocumentsRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<PaymentRetailDocumentModelDB[]>>(TransmissionQueues.GetPaymentsDocumentsRetailReceive, req, token: token) ?? new();
    #endregion

    #region Payments orders link`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentOrderLinkDocumentAsync(PaymentOrderRetailLinkModelDB req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreatePaymentOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentOrderLinkDocumentAsync(PaymentOrderRetailLinkModelDB req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdatePaymentOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentOrderRetailLinkModelDB>> SelectPaymentsOrdersDocumentsLinksAsync(TPaginationRequestStandardModel<SelectPaymentsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<PaymentOrderRetailLinkModelDB>>(TransmissionQueues.SelectPaymentsOrdersDocumentsLinksRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeletePaymentOrderLinkDocumentAsync(DeletePaymentOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeletePaymentOrderLinkDocumentReceive, req, token: token) ?? new();
    #endregion

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

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ToggleWalletTypeDisabledForPaymentTypeAsync(ToggleWalletTypeDisabledForPaymentTypeRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.ToggleWalletTypeDisabledForPaymentTypeRetailReceive, req, token: token) ?? new();
    #endregion

    #region Wallet`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateWalletRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateWalletRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WalletRetailModelDB>>(TransmissionQueues.SelectWalletsRetailReceive, req, token: token) ?? new();
    #endregion

    #region Conversion`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionDocumentRetailAsync(CreateWalletConversionRetailDocumentRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateConversionDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConversionDocumentRetailAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateConversionDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsRetailAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>>(TransmissionQueues.SelectConversionsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WalletConversionRetailDocumentModelDB[]>> GetConversionsDocumentsRetailAsync(ReadWalletsRetailsConversionDocumentsRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<WalletConversionRetailDocumentModelDB[]>>(TransmissionQueues.GetConversionsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteToggleConversionRetailAsync(int conversionId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteToggleConversionRetailReceive, conversionId, token: token) ?? new();
    #endregion

    #region Conversions orders link`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionOrderLinkDocumentRetailAsync(ConversionOrderRetailLinkModelDB req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateConversionOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConversionOrderLinkDocumentRetailAsync(ConversionOrderRetailLinkModelDB req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateConversionOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ConversionOrderRetailLinkModelDB>> SelectConversionsOrdersDocumentsLinksRetailAsync(TPaginationRequestStandardModel<SelectConversionsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<ConversionOrderRetailLinkModelDB>>(TransmissionQueues.SelectConversionsOrdersDocumentsLinksRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteConversionOrderLinkDocumentRetailAsync(DeleteConversionOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteConversionOrderLinkDocumentRetailReceive, req, token: token) ?? new();
    #endregion

    #region Report`s
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailReportRowModel>> FinancialsReportRetailAsync(TPaginationRequestStandardModel<SelectPaymentsRetailReportRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<WalletRetailReportRowModel>>(TransmissionQueues.FinancialsReportRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OffersRetailReportRowModel>> OffersOfOrdersReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfOrdersRetailReportRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<OffersRetailReportRowModel>>(TransmissionQueues.OffersOfOrdersReportRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<MainReportResponseModel> GetMainReportAsync(MainReportRequestModel req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<MainReportResponseModel>(TransmissionQueues.GetMainReportRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OffersRetailReportRowModel>> OffersOfDeliveriesReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<OffersRetailReportRowModel>>(TransmissionQueues.OffersOfDeliveriesReportRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<PeriodBaseModel> AboutPeriodAsync(object? req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<PeriodBaseModel>(TransmissionQueues.AboutPeriodRetailReceive, req, token: token) ?? new();
    #endregion
}