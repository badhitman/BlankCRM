////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using MQTTCallLib;
using Transmission.Receives.StockSharpDriver;
using Transmission.Receives.storage;

namespace StockSharpService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection StockSharpRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<GoToPageForRowReceive, TPaginationRequestStandardModel<int>, TPaginationResponseModel<NLogRecordModelDB>>()
            .RegisterMqListener<MetadataLogsReceive, PeriodDatesTimesModel, TResponseModel<LogsMetadataResponseModel>>()
            .RegisterMqListener<LogsSelectReceive, TPaginationRequestStandardModel<LogsSelectRequestModel>, TPaginationResponseModel<NLogRecordModelDB>>()

            .RegisterMqListener<PingStockSharpDriverReceive, object, ResponseBaseModel>()
            .RegisterMqListener<GetPortfoliosStockSharpDriverReceive, int[], TResponseModel<List<PortfolioStockSharpModel>>>()
            .RegisterMqListener<GetBoardsStockSharpDriverReceive, int[], TResponseModel<List<BoardStockSharpModel>>>()
            .RegisterMqListener<GetExchangesStockSharpDriverReceive, int[], TResponseModel<List<ExchangeStockSharpModel>>>()
            .RegisterMqListener<GetInstrumentsStockSharpDriverReceive, int[], TResponseModel<List<InstrumentTradeStockSharpModel>>>()
            .RegisterMqListener<GetOrdersStockSharpDriverReceive, int[], TResponseModel<List<OrderStockSharpModel>>>()
            .RegisterMqListener<OrderRegisterStockSharpDriverReceive, CreateOrderRequestModel, ResponseBaseModel>()
            .RegisterMqListener<InstrumentFavoriteToggleReceive, InstrumentTradeStockSharpViewModel, ResponseBaseModel>()
            .RegisterMqListener<InstrumentsSelectStockSharpDriverReceive, TPaginationRequestStandardModel<InstrumentsRequestModel>, TPaginationResponseModel<InstrumentTradeStockSharpViewModel>>()
            ;
    }
}