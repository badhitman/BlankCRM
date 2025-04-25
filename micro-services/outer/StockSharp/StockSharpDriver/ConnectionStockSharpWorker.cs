////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using StockSharp.Algo;
using StockSharp.BusinessEntities;

namespace StockSharpDriver;

/// <inheritdoc/>
public class ConnectionStockSharpWorker(
    StockSharpClientConfigModel conf,
    ILogger<ConnectionStockSharpWorker> _logger,
    StockSharp.Algo.Connector _connector) : BackgroundService
{
    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _connector.Connected += ConnectedHandle;
        _connector.ConnectedEx += ConnectedExHandle;
        _connector.Disconnected += DisconnectedHandle;
        _connector.BoardReceived += BoardReceivedHandle;
        _connector.CandleReceived += CandleReceivedHandle;
        _connector.ConnectionLost += ConnectionLostHandle;
        _connector.ConnectionError += ConnectionErrorHandle;
        _connector.DataTypeReceived += DataTypeReceivedHandle;
        _connector.ConnectionErrorEx += ConnectionErrorExHandle;
        _connector.ConnectionRestored += ConnectionRestoredHandle;
        _connector.CurrentTimeChanged += CurrentTimeChangedHandle;
        _connector.ChangePasswordResult += ChangePasswordResultHandle;
        _connector.DisconnectedEx += DisconnectedExHandle;
        _connector.Disposed += DisposedHandle;
        _connector.Error += ErrorHandle;
        _connector.Level1Received += Level1ReceivedHandle;
        _connector.Log += LogHandle;
        _connector.LookupPortfoliosResult += LookupPortfoliosResultHandle;
        _connector.LookupSecuritiesResult += LookupSecuritiesResultHandle;
        _connector.MassOrderCanceled += MassOrderCanceledHandle;
        _connector.MassOrderCanceled2 += MassOrderCanceled2Handle;
        _connector.MassOrderCancelFailed += MassOrderCancelFailedHandle;
        _connector.MassOrderCancelFailed2 += MassOrderCancelFailed2Handle;
        _connector.NewMessage += NewMessageHandle;
        _connector.NewsReceived += NewsReceivedHandle;
        _connector.OrderBookReceived += OrderBookReceivedHandle;
        _connector.OrderCancelFailReceived += OrderCancelFailReceivedHandle;
        _connector.OrderEditFailReceived += OrderEditFailReceivedHandle;
        _connector.OrderLogReceived += OrderLogReceivedHandle;
        _connector.OrderReceived += OrderReceivedHandle;
        _connector.OrderRegisterFailReceived += OrderRegisterFailReceivedHandle;
        _connector.OwnTradeReceived += OwnTradeReceivedHandle;
        _connector.ParentRemoved += ParentRemovedHandle;
        _connector.PortfolioReceived += PortfolioReceivedHandle;
        _connector.PositionReceived += PositionReceivedHandle;
        _connector.SecurityReceived += SecurityReceivedHandle;
        _connector.SubscriptionFailed += SubscriptionFailedHandle;
        _connector.SubscriptionOnline += SubscriptionOnlineHandle;
        _connector.SubscriptionReceived += SubscriptionReceivedHandle;
        _connector.SubscriptionStarted += SubscriptionStartedHandle;
        _connector.SubscriptionStopped += SubscriptionStoppedHandle;
        _connector.TickTradeReceived += TickTradeReceivedHandle;
        _connector.ValuesChanged += ValuesChangedHandle;

        _connector.Connect();
        while (!stoppingToken.IsCancellationRequested)
        {
            //logger.LogDebug();
            await Task.Delay(1000, stoppingToken);
        }

        _logger.LogInformation($"call - {nameof(_connector.CancelOrders)}!");
        _connector.CancelOrders();

        foreach (Subscription? sub in _connector.Subscriptions)
        {
            _connector.UnSubscribe(sub);
            _logger.LogInformation($"{nameof(_connector.UnSubscribe)} > {sub.GetType().FullName}");
        }

        await _connector.DisconnectAsync(stoppingToken);

        _connector.Connected -= ConnectedHandle;
        _connector.ConnectedEx -= ConnectedExHandle;
        _connector.Disconnected -= DisconnectedHandle;
        _connector.BoardReceived -= BoardReceivedHandle;
        _connector.CandleReceived -= CandleReceivedHandle;
        _connector.ConnectionLost -= ConnectionLostHandle;
        _connector.ConnectionError -= ConnectionErrorHandle;
        _connector.DataTypeReceived -= DataTypeReceivedHandle;
        _connector.ConnectionErrorEx -= ConnectionErrorExHandle;
        _connector.ConnectionRestored -= ConnectionRestoredHandle;
        _connector.CurrentTimeChanged -= CurrentTimeChangedHandle;
        _connector.ChangePasswordResult -= ChangePasswordResultHandle;
        _connector.DisconnectedEx -= DisconnectedExHandle;
        _connector.Disposed -= DisposedHandle;
        _connector.Error -= ErrorHandle;
        _connector.Level1Received -= Level1ReceivedHandle;
        _connector.Log -= LogHandle;
        _connector.LookupPortfoliosResult -= LookupPortfoliosResultHandle;
        _connector.LookupSecuritiesResult -= LookupSecuritiesResultHandle;
        _connector.MassOrderCanceled -= MassOrderCanceledHandle;
        _connector.MassOrderCanceled2 -= MassOrderCanceled2Handle;
        _connector.MassOrderCancelFailed -= MassOrderCancelFailedHandle;
        _connector.MassOrderCancelFailed2 -= MassOrderCancelFailed2Handle;
        _connector.NewMessage -= NewMessageHandle;
        _connector.NewsReceived -= NewsReceivedHandle;
        _connector.OrderBookReceived -= OrderBookReceivedHandle;
        _connector.OrderCancelFailReceived -= OrderCancelFailReceivedHandle;
        _connector.OrderEditFailReceived -= OrderEditFailReceivedHandle;
        _connector.OrderLogReceived -= OrderLogReceivedHandle;
        _connector.OrderReceived -= OrderReceivedHandle;
        _connector.OrderRegisterFailReceived -= OrderRegisterFailReceivedHandle;
        _connector.OwnTradeReceived -= OwnTradeReceivedHandle;
        _connector.ParentRemoved -= ParentRemovedHandle;
        _connector.PortfolioReceived -= PortfolioReceivedHandle;
        _connector.PositionReceived -= PositionReceivedHandle;
        _connector.SecurityReceived -= SecurityReceivedHandle;
        _connector.SubscriptionFailed -= SubscriptionFailedHandle;
        _connector.SubscriptionOnline -= SubscriptionOnlineHandle;
        _connector.SubscriptionReceived -= SubscriptionReceivedHandle;
        _connector.SubscriptionStarted -= SubscriptionStartedHandle;
        _connector.SubscriptionStopped -= SubscriptionStoppedHandle;
        _connector.TickTradeReceived -= TickTradeReceivedHandle;
        _connector.ValuesChanged -= ValuesChangedHandle;
    }

    private void ValuesChangedHandle(Security arg1, IEnumerable<KeyValuePair<StockSharp.Messages.Level1Fields, object>> arg2, DateTimeOffset arg3, DateTimeOffset arg4)
    {
        throw new NotImplementedException();
    }

    private void TickTradeReceivedHandle(Subscription arg1, StockSharp.Messages.ITickTradeMessage arg2)
    {
        throw new NotImplementedException();
    }

    private void SubscriptionStoppedHandle(Subscription arg1, Exception arg2)
    {
        throw new NotImplementedException();
    }

    private void SubscriptionStartedHandle(Subscription obj)
    {
        throw new NotImplementedException();
    }

    private void SubscriptionReceivedHandle(Subscription arg1, object arg2)
    {
        throw new NotImplementedException();
    }

    private void SubscriptionOnlineHandle(Subscription obj)
    {
        throw new NotImplementedException();
    }

    private void SubscriptionFailedHandle(Subscription arg1, Exception arg2, bool arg3)
    {
        throw new NotImplementedException();
    }

    private void SecurityReceivedHandle(Subscription arg1, Security arg2)
    {
        throw new NotImplementedException();
    }

    private void PositionReceivedHandle(Subscription arg1, Position arg2)
    {
        throw new NotImplementedException();
    }

    private void PortfolioReceivedHandle(Subscription arg1, Portfolio arg2)
    {
        throw new NotImplementedException();
    }

    private void ParentRemovedHandle(Ecng.Logging.ILogSource obj)
    {
        throw new NotImplementedException();
    }

    private void OwnTradeReceivedHandle(Subscription arg1, MyTrade arg2)
    {
        throw new NotImplementedException();
    }

    private void OrderRegisterFailReceivedHandle(Subscription arg1, OrderFail arg2)
    {
        throw new NotImplementedException();
    }

    private void OrderReceivedHandle(Subscription arg1, Order arg2)
    {
        throw new NotImplementedException();
    }

    private void OrderLogReceivedHandle(Subscription arg1, StockSharp.Messages.IOrderLogMessage arg2)
    {
        throw new NotImplementedException();
    }

    private void OrderEditFailReceivedHandle(Subscription arg1, OrderFail arg2)
    {
        throw new NotImplementedException();
    }

    private void OrderCancelFailReceivedHandle(Subscription arg1, OrderFail arg2)
    {
        throw new NotImplementedException();
    }

    private void OrderBookReceivedHandle(Subscription arg1, StockSharp.Messages.IOrderBookMessage arg2)
    {
        throw new NotImplementedException();
    }

    private void NewsReceivedHandle(Subscription arg1, News arg2)
    {
        throw new NotImplementedException();
    }

    private void NewMessageHandle(StockSharp.Messages.Message obj)
    {
        throw new NotImplementedException();
    }

    private void MassOrderCancelFailed2Handle(long arg1, Exception arg2, DateTimeOffset arg3)
    {
        throw new NotImplementedException();
    }

    private void MassOrderCancelFailedHandle(long arg1, Exception arg2)
    {
        throw new NotImplementedException();
    }

    private void MassOrderCanceled2Handle(long arg1, DateTimeOffset arg2)
    {
        throw new NotImplementedException();
    }

    private void MassOrderCanceledHandle(long obj)
    {
        throw new NotImplementedException();
    }

    private void LookupSecuritiesResultHandle(StockSharp.Messages.SecurityLookupMessage arg1, IEnumerable<Security> arg2, Exception arg3)
    {
        throw new NotImplementedException();
    }

    private void LookupPortfoliosResultHandle(StockSharp.Messages.PortfolioLookupMessage arg1, IEnumerable<Portfolio> arg2, Exception arg3)
    {
        throw new NotImplementedException();
    }

    private void LogHandle(Ecng.Logging.LogMessage obj)
    {
        throw new NotImplementedException();
    }

    private void Level1ReceivedHandle(Subscription arg1, StockSharp.Messages.Level1ChangeMessage arg2)
    {
        throw new NotImplementedException();
    }

    private void ErrorHandle(Exception obj)
    {
        throw new NotImplementedException();
    }

    private void DisposedHandle()
    {
        throw new NotImplementedException();
    }

    private void DisconnectedExHandle(StockSharp.Messages.IMessageAdapter obj)
    {
        throw new NotImplementedException();
    }

    private void DisconnectedHandle()
    {
        throw new NotImplementedException();
    }

    private void DataTypeReceivedHandle(Subscription arg1, StockSharp.Messages.DataType arg2)
    {
        throw new NotImplementedException();
    }

    private void CurrentTimeChangedHandle(TimeSpan obj)
    {
        throw new NotImplementedException();
    }

    private void ConnectionRestoredHandle(StockSharp.Messages.IMessageAdapter obj)
    {
        throw new NotImplementedException();
    }

    private void ConnectionLostHandle(StockSharp.Messages.IMessageAdapter obj)
    {
        throw new NotImplementedException();
    }

    private void ConnectionErrorExHandle(StockSharp.Messages.IMessageAdapter arg1, Exception arg2)
    {
        throw new NotImplementedException();
    }

    private void ConnectionErrorHandle(Exception obj)
    {
        throw new NotImplementedException();
    }

    private void ConnectedExHandle(StockSharp.Messages.IMessageAdapter obj)
    {
        throw new NotImplementedException();
    }

    private void ConnectedHandle()
    {
        throw new NotImplementedException();
    }

    private void ChangePasswordResultHandle(long arg1, Exception arg2)
    {
        throw new NotImplementedException();
    }

    void CandleReceivedHandle(Subscription subscription, StockSharp.Messages.ICandleMessage candleMessage)
    {
        throw new NotImplementedException();
    }

    void BoardReceivedHandle(Subscription subscription, ExchangeBoard boardExchange)
    {
        throw new NotImplementedException();
    }
}