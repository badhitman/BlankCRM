////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using MQTTCallLib;
using Transmission.Receives.StockSharpDriver;

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
            .RegisterMqListener<PingStockSharpDriverReceive, object, ResponseBaseModel>()
            .RegisterMqListener<GetPortfoliosStockSharpDriverReceive, int[], TResponseModel<List<PortfolioStockSharpModel>>>()
            .RegisterMqListener<GetBoardsStockSharpDriverReceive, int[], TResponseModel<List<BoardStockSharpModel>>>()
            .RegisterMqListener<GetExchangesStockSharpDriverReceive, int[], TResponseModel<List<ExchangeStockSharpModel>>>()
            .RegisterMqListener<GetInstrumentsStockSharpDriverReceive, int[], TResponseModel<List<InstrumentTradeStockSharpModel>>>()
            .RegisterMqListener<GetOrdersStockSharpDriverReceive, int[], TResponseModel<List<OrderStockSharpModel>>>()
            .RegisterMqListener<InstrumentsSelectStockSharpDriverReceive, TPaginationRequestStandardModel<InstrumentsRequestModel>, TPaginationResponseModel<InstrumentTradeStockSharpModel>>()
            ;
    }
}