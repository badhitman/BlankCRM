////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using SharedLib;
using static SharedLib.GlobalStaticConstantsTransmission;

namespace RemoteCallLib;

/// <summary>
/// Розница
/// </summary>
public class RetailTransmission([FromKeyedServices(nameof(RabbitClient))] IMQStandardClientRPC rabbitClient) : IRetailService
{
    #region Order`s (document retail)
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRetailDocumentAsync(TAuthRequestStandardModel<CreateDocumentRetailRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid>> UpdateRetailDocumentAsync(TAuthRequestStandardModel<DocumentRetailModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Guid>>(TransmissionQueues.UpdateDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentRetailModelDB[]>> RetailDocumentsGetAsync(RetailDocumentsGetRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentRetailModelDB[]>>(TransmissionQueues.DocumentsGetRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DocumentRetailModelDB>> SelectRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<DocumentRetailModelDB>>(TransmissionQueues.SelectDocumentsRetailReceive, req, token: token) ?? new();
    #endregion

    #region Rows for retail order (document)
    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> CreateRowRetailDocumentAsync(TAuthRequestStandardModel<RowOfRetailOrderDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<DocumentNewVersionResponseModel>(TransmissionQueues.CreateRowDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdateRowRetailDocumentAsync(TAuthRequestStandardModel<RowOfRetailOrderDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Guid?>>(TransmissionQueues.UpdateRowDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RowOfRetailOrderDocumentModelDB>> SelectRowsRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<RowOfRetailOrderDocumentModelDB>>(TransmissionQueues.SelectRowsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<DeleteRowRetailDocumentResponseModel> DeleteRowRetailDocumentAsync(TAuthRequestStandardModel<DeleteRowRetailDocumentRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<DeleteRowRetailDocumentResponseModel>(TransmissionQueues.DeleteRowDocumentRetailReceive, req, token: token) ?? new();
    #endregion

    #region Statuses (of order`s document)
    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> CreateOrderStatusDocumentAsync(TAuthRequestStandardModel<OrderStatusRetailDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<DocumentNewVersionResponseModel>(TransmissionQueues.CreateOrderStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdateOrderStatusDocumentAsync(TAuthRequestStandardModel<OrderStatusRetailDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Guid?>>(TransmissionQueues.UpdateOrderStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OrderStatusRetailDocumentModelDB>> SelectOrderDocumentStatusesAsync(TPaginationRequestStandardModel<SelectOrderStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<OrderStatusRetailDocumentModelDB>>(TransmissionQueues.SelectOrderDocumentStatusesRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> DeleteOrderStatusDocumentAsync(TAuthRequestStandardModel<DeleteOrderStatusDocumentRequestModel> statusId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<DocumentNewVersionResponseModel>(TransmissionQueues.DeleteOrderStatusDocumentRetailReceive, statusId, token: token) ?? new();
    #endregion

    #region Delivery Document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryDocumentAsync(TAuthRequestStandardModel<CreateDeliveryDocumentRetailRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdateDeliveryDocumentAsync(TAuthRequestStandardModel<DeliveryDocumentRetailModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Guid?>>(TransmissionQueues.UpdateDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<DeliveryDocumentRetailModelDB>>(TransmissionQueues.SelectDeliveryDocumentsRetailReceive, req, token: token) ?? new();
    /// <inheritdoc/>
    public async Task<TResponseModel<DeliveryDocumentRetailModelDB[]>> GetDeliveryDocumentsAsync(GetDeliveryDocumentsRetailRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<DeliveryDocumentRetailModelDB[]>>(TransmissionQueues.GetDeliveryDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<FileAttachModel> GetDeliveriesJournalFileAsync(SelectDeliveryDocumentsRetailRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<FileAttachModel>(TransmissionQueues.GetDeliveriesJournalFileRetailReceive, req, token: token) ?? new();
    #endregion

    #region Row Of Delivery Document
    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> CreateRowOfDeliveryDocumentAsync(TAuthRequestStandardModel<RowOfDeliveryRetailDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<DocumentNewVersionResponseModel>(TransmissionQueues.CreateRowOfDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdateRowOfDeliveryDocumentAsync(TAuthRequestStandardModel<RowOfDeliveryRetailDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Guid?>>(TransmissionQueues.UpdateRowOfDeliveryDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowsOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<RowOfDeliveryRetailDocumentModelDB>>(TransmissionQueues.SelectRowsOfDeliveryDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> DeleteRowOfDeliveryDocumentAsync(TAuthRequestStandardModel<DeleteRowOfDeliveryDocumentRequestModel> rowId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<DocumentNewVersionResponseModel>(TransmissionQueues.DeleteRowOfDeliveryDocumentRetailReceive, rowId, token: token) ?? new();
    #endregion

    #region Statuses (of delivery document)
    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel> CreateDeliveryStatusDocumentAsync(TAuthRequestStandardModel<DeliveryStatusRetailDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<DocumentNewVersionResponseModel>(TransmissionQueues.CreateDeliveryStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdateDeliveryStatusDocumentAsync(TAuthRequestStandardModel<DeliveryStatusRetailDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Guid?>>(TransmissionQueues.UpdateDeliveryStatusDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<DeliveryStatusRetailDocumentModelDB>>(TransmissionQueues.SelectDeliveryStatusesDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<DeleteDeliveryStatusDocumentResponseModel> DeleteDeliveryStatusDocumentAsync(TAuthRequestStandardModel<DeleteDeliveryStatusDocumentRequestModel> statusId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<DeleteDeliveryStatusDocumentResponseModel>(TransmissionQueues.DeleteDeliveryStatusDocumentRetailReceive, statusId, token: token) ?? new();
    #endregion

    #region Deliveries orders link`s 
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryOrderLinkDocumentAsync(TAuthRequestStandardModel<RetailOrderDeliveryLinkModelDB> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateDeliveryOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<decimal>> TotalWeightOrdersDocumentsLinksAsync(TotalWeightDeliveriesOrdersLinksDocumentsRequestModel req, CancellationToken token = default)
      => await rabbitClient.MqRemoteCallAsync<TResponseModel<decimal>>(TransmissionQueues.TotalWeightOrdersLinksDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryOrderLinkDocumentAsync(TAuthRequestStandardModel<RetailOrderDeliveryLinkModelDB> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateDeliveryOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RetailOrderDeliveryLinkModelDB>> SelectDeliveriesOrdersLinksDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<RetailOrderDeliveryLinkModelDB>>(TransmissionQueues.SelectDeliveriesOrdersLinksDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<RetailOrderDeliveryLinkModelDB[]>> DeliveriesOrdersLinksDocumentsReadAsync(int[] req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<RetailOrderDeliveryLinkModelDB[]>>(TransmissionQueues.DeliveriesOrdersLinksDocumentsReadRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDeliveryOrderLinkDocumentAsync(TAuthRequestStandardModel<OrderDeliveryModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteDeliveryOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<decimal>> GetSumConversionsOrdersAmountsAsync(GetSumConversionsOrdersAmountsRequestModel req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<decimal>>(TransmissionQueues.GetSumConversionsOrdersAmountsRetailReceive, req, token: token) ?? new();
    #endregion

    #region Payment Document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentDocumentAsync(TAuthRequestStandardModel<CreatePaymentRetailDocumentRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreatePaymentDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdatePaymentDocumentAsync(TAuthRequestStandardModel<PaymentRetailDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Guid?>>(TransmissionQueues.UpdatePaymentDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<PaymentRetailDocumentModelDB>>(TransmissionQueues.SelectPaymentsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentRetailDocumentModelDB[]>> GetPaymentsDocumentsAsync(GetPaymentsRetailOrdersDocumentsRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<PaymentRetailDocumentModelDB[]>>(TransmissionQueues.GetPaymentsDocumentsRetailReceive, req, token: token) ?? new();
    #endregion

    #region Payments orders link`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentOrderLinkDocumentAsync(TAuthRequestStandardModel<PaymentOrderRetailLinkModelDB> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreatePaymentOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentOrderLinkDocumentAsync(TAuthRequestStandardModel<PaymentOrderRetailLinkModelDB> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdatePaymentOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<PaymentOrderRetailLinkModelDB>> SelectPaymentsOrdersDocumentsLinksAsync(TPaginationRequestStandardModel<SelectPaymentsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<PaymentOrderRetailLinkModelDB>>(TransmissionQueues.SelectPaymentsOrdersDocumentsLinksRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentOrderRetailLinkModelDB[]>> PaymentsOrdersDocumentsLinksGetAsync(int[] req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<PaymentOrderRetailLinkModelDB[]>>(TransmissionQueues.PaymentsOrdersDocumentsLinksGetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeletePaymentOrderLinkDocumentAsync(TAuthRequestStandardModel<OrderPaymentModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeletePaymentOrderLinkDocumentReceive, req, token: token) ?? new();
    #endregion

    #region Wallet Type
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletTypeAsync(TAuthRequestStandardModel<WalletRetailTypeModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateWalletTypeRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateWalletTypeAsync(TAuthRequestStandardModel<WalletRetailTypeModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateWalletTypeRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WalletRetailTypeViewModel[]>> WalletsTypesGetAsync(int[] reqIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<WalletRetailTypeViewModel[]>>(TransmissionQueues.WalletsTypesGetRetailReceive, reqIds, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WalletRetailTypeViewModel>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<WalletRetailTypeViewModel>>(TransmissionQueues.SelectWalletsTypesRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ToggleWalletTypeDisabledForPaymentTypeAsync(TAuthRequestStandardModel<ToggleWalletTypeDisabledForPaymentTypeRequestModel> req, CancellationToken token = default)
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
    public async Task<TPaginationResponseStandardModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<WalletRetailModelDB>>(TransmissionQueues.SelectWalletsRetailReceive, req, token: token) ?? new();
    #endregion

    #region Conversion`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionDocumentRetailAsync(TAuthRequestStandardModel<CreateWalletConversionRetailDocumentRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateConversionDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdateConversionDocumentRetailAsync(TAuthRequestStandardModel<WalletConversionRetailDocumentModelDB> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Guid?>>(TransmissionQueues.UpdateConversionDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsRetailAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<WalletConversionRetailDocumentModelDB>>(TransmissionQueues.SelectConversionsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WalletConversionRetailDocumentModelDB[]>> GetConversionsDocumentsRetailAsync(ReadWalletsRetailsConversionDocumentsRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<WalletConversionRetailDocumentModelDB[]>>(TransmissionQueues.GetConversionsDocumentsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> DeleteToggleConversionRetailAsync(TAuthRequestStandardModel<DeleteToggleConversionRequestModel> conversionId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Guid?>>(TransmissionQueues.DeleteToggleConversionRetailReceive, conversionId, token: token) ?? new();
    #endregion

    #region Conversions orders link`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionOrderLinkDocumentRetailAsync(TAuthRequestStandardModel<OrderConversionAmountModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(TransmissionQueues.CreateConversionOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConversionOrderLinkDocumentRetailAsync(TAuthRequestStandardModel<OrderConversionAmountModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.UpdateConversionOrderLinkDocumentRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ConversionOrderRetailLinkModelDB>> SelectConversionsOrdersDocumentsLinksRetailAsync(TPaginationRequestStandardModel<SelectConversionsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<ConversionOrderRetailLinkModelDB>>(TransmissionQueues.SelectConversionsOrdersDocumentsLinksRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ConversionOrderRetailLinkModelDB[]>> ConversionsOrdersDocumentsLinksReadRetailAsync(int[] req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<ConversionOrderRetailLinkModelDB[]>>(TransmissionQueues.ConversionsOrdersDocumentsLinksRetailReadReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteConversionOrderLinkDocumentRetailAsync(TAuthRequestStandardModel<OrderConversionModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(TransmissionQueues.DeleteConversionOrderLinkDocumentRetailReceive, req, token: token) ?? new();
    #endregion

    #region Report`s
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WalletRetailReportRowModel>> FinancialsReportRetailAsync(TPaginationRequestStandardModel<SelectPaymentsRetailReportRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<WalletRetailReportRowModel>>(TransmissionQueues.FinancialsReportRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OffersRetailReportRowModel>> OffersOfOrdersReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfOrdersRetailReportRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<OffersRetailReportRowModel>>(TransmissionQueues.OffersOfOrdersReportRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<MainReportResponseModel> GetMainReportAsync(MainReportRequestModel req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<MainReportResponseModel>(TransmissionQueues.GetMainReportRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OffersRetailReportRowModel>> OffersOfDeliveriesReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<OffersRetailReportRowModel>>(TransmissionQueues.OffersOfDeliveriesReportRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<PeriodBaseModel> AboutPeriodAsync(object? req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<PeriodBaseModel>(TransmissionQueues.AboutPeriodRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DocumentRetailModelDB>> SelectRowsDocumentsForMainReportRetailAsync(TPaginationRequestStandardModel<MainReportRequestModel> req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<DocumentRetailModelDB>>(TransmissionQueues.SelectRowsRetailDocumentsForMainReportRetailReceive, req, token: token) ?? new();
    #endregion
}