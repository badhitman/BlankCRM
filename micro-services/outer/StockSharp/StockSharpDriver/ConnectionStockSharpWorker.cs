////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using StockSharp.Algo;
using StockSharp.Algo.Indicators;
using StockSharp.BusinessEntities;
using System.Linq;

namespace StockSharpDriver;

/// <inheritdoc/>
public class ConnectionStockSharpWorker(
    //StockSharpClientConfigModel conf,
    IStockSharpDataService dataRepo,
    ILogger<ConnectionStockSharpWorker> _logger,
    IStockSharpEventsService eventTrans,
    Connector Connector) : BackgroundService
{
    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Connector.Connected += ConnectedHandle;
        Connector.ConnectedEx += ConnectedExHandle;
        Connector.Disconnected += DisconnectedHandle;
        Connector.BoardReceived += BoardReceivedHandle;
        Connector.CandleReceived += CandleReceivedHandle;
        Connector.ConnectionLost += ConnectionLostHandle;
        Connector.ConnectionError += ConnectionErrorHandle;
        Connector.DataTypeReceived += DataTypeReceivedHandle;
        Connector.ConnectionErrorEx += ConnectionErrorExHandle;
        Connector.ConnectionRestored += ConnectionRestoredHandle;
        Connector.CurrentTimeChanged += CurrentTimeChangedHandle;
        Connector.ChangePasswordResult += ChangePasswordResultHandle;
        Connector.DisconnectedEx += DisconnectedExHandle;
        Connector.Disposed += DisposedHandle;
        Connector.Error += ErrorHandle;
        Connector.Level1Received += Level1ReceivedHandle;
        Connector.Log += LogHandle;
        Connector.LookupPortfoliosResult += LookupPortfoliosResultHandle;
        Connector.LookupSecuritiesResult += LookupSecuritiesResultHandle;
        Connector.MassOrderCanceled += MassOrderCanceledHandle;
        Connector.MassOrderCanceled2 += MassOrderCanceled2Handle;
        Connector.MassOrderCancelFailed += MassOrderCancelFailedHandle;
        Connector.MassOrderCancelFailed2 += MassOrderCancelFailed2Handle;
        Connector.NewMessage += NewMessageHandle;
        Connector.NewsReceived += NewsReceivedHandle;
        Connector.OrderBookReceived += OrderBookReceivedHandle;
        Connector.OrderCancelFailReceived += OrderCancelFailReceivedHandle;
        Connector.OrderEditFailReceived += OrderEditFailReceivedHandle;
        Connector.OrderLogReceived += OrderLogReceivedHandle;
        Connector.OrderReceived += OrderReceivedHandle;
        Connector.OrderRegisterFailReceived += OrderRegisterFailReceivedHandle;
        Connector.OwnTradeReceived += OwnTradeReceivedHandle;
        Connector.ParentRemoved += ParentRemovedHandle;
        Connector.PortfolioReceived += PortfolioReceivedHandle;
        Connector.PositionReceived += PositionReceivedHandle;
        Connector.SecurityReceived += SecurityReceivedHandle;
        Connector.SubscriptionFailed += SubscriptionFailedHandle;
        Connector.SubscriptionOnline += SubscriptionOnlineHandle;
        Connector.SubscriptionReceived += SubscriptionReceivedHandle;
        Connector.SubscriptionStarted += SubscriptionStartedHandle;
        Connector.SubscriptionStopped += SubscriptionStoppedHandle;
        Connector.TickTradeReceived += TickTradeReceivedHandle;
        Connector.ValuesChanged += ValuesChangedHandle;

        Connector.Connect();
        while (!stoppingToken.IsCancellationRequested)
        {
            //logger.LogDebug();
            await Task.Delay(1000, stoppingToken);
        }

        _logger.LogInformation($"call - {nameof(Connector.CancelOrders)}!");
        Connector.CancelOrders();

        foreach (Subscription sub in Connector.Subscriptions)
        {
            Connector.UnSubscribe(sub);
            _logger.LogInformation($"{nameof(Connector.UnSubscribe)} > {sub.GetType().FullName}");
        }

        await Connector.DisconnectAsync(stoppingToken);

        Connector.Connected -= ConnectedHandle;
        Connector.ConnectedEx -= ConnectedExHandle;
        Connector.Disconnected -= DisconnectedHandle;
        Connector.BoardReceived -= BoardReceivedHandle;
        Connector.CandleReceived -= CandleReceivedHandle;
        Connector.ConnectionLost -= ConnectionLostHandle;
        Connector.ConnectionError -= ConnectionErrorHandle;
        Connector.DataTypeReceived -= DataTypeReceivedHandle;
        Connector.ConnectionErrorEx -= ConnectionErrorExHandle;
        Connector.ConnectionRestored -= ConnectionRestoredHandle;
        Connector.CurrentTimeChanged -= CurrentTimeChangedHandle;
        Connector.ChangePasswordResult -= ChangePasswordResultHandle;
        Connector.DisconnectedEx -= DisconnectedExHandle;
        Connector.Disposed -= DisposedHandle;
        Connector.Error -= ErrorHandle;
        Connector.Level1Received -= Level1ReceivedHandle;
        Connector.Log -= LogHandle;
        Connector.LookupPortfoliosResult -= LookupPortfoliosResultHandle;
        Connector.LookupSecuritiesResult -= LookupSecuritiesResultHandle;
        Connector.MassOrderCanceled -= MassOrderCanceledHandle;
        Connector.MassOrderCanceled2 -= MassOrderCanceled2Handle;
        Connector.MassOrderCancelFailed -= MassOrderCancelFailedHandle;
        Connector.MassOrderCancelFailed2 -= MassOrderCancelFailed2Handle;
        Connector.NewMessage -= NewMessageHandle;
        Connector.NewsReceived -= NewsReceivedHandle;
        Connector.OrderBookReceived -= OrderBookReceivedHandle;
        Connector.OrderCancelFailReceived -= OrderCancelFailReceivedHandle;
        Connector.OrderEditFailReceived -= OrderEditFailReceivedHandle;
        Connector.OrderLogReceived -= OrderLogReceivedHandle;
        Connector.OrderReceived -= OrderReceivedHandle;
        Connector.OrderRegisterFailReceived -= OrderRegisterFailReceivedHandle;
        Connector.OwnTradeReceived -= OwnTradeReceivedHandle;
        Connector.ParentRemoved -= ParentRemovedHandle;
        Connector.PortfolioReceived -= PortfolioReceivedHandle;
        Connector.PositionReceived -= PositionReceivedHandle;
        Connector.SecurityReceived -= SecurityReceivedHandle;
        Connector.SubscriptionFailed -= SubscriptionFailedHandle;
        Connector.SubscriptionOnline -= SubscriptionOnlineHandle;
        Connector.SubscriptionReceived -= SubscriptionReceivedHandle;
        Connector.SubscriptionStarted -= SubscriptionStartedHandle;
        Connector.SubscriptionStopped -= SubscriptionStoppedHandle;
        Connector.TickTradeReceived -= TickTradeReceivedHandle;
        Connector.ValuesChanged -= ValuesChangedHandle;
    }

    void ValuesChangedHandle(Security instrument, IEnumerable<KeyValuePair<StockSharp.Messages.Level1Fields, object>> dataPayload, DateTimeOffset dtOffsetMaster, DateTimeOffset dtOffsetSlave)
    {
        ConnectorValuesChangedEventPayloadModel req = new()
        {
            OffsetSlave = dtOffsetSlave,
            OffsetMaster = dtOffsetMaster,
            DataPayload = [.. dataPayload.Select(x => new KeyValuePair<Level1FieldsStockSharpEnum, object>((Level1FieldsStockSharpEnum)Enum.Parse(typeof(Level1FieldsStockSharpEnum), Enum.GetName(x.Key)!), x.Value))],
            Instrument = new InstrumentTradeModel().Bind(instrument),
        };
        dataRepo.SaveInstrument(req.Instrument);
        eventTrans.ValuesChanged(req);
    }

    void TickTradeReceivedHandle(Subscription subscription, StockSharp.Messages.ITickTradeMessage msg)
    {
        throw new NotImplementedException();
    }

    void SubscriptionStoppedHandle(Subscription subscription, Exception ex)
    {
        throw new NotImplementedException();
    }

    void SubscriptionStartedHandle(Subscription subscription)
    {
        throw new NotImplementedException();
    }

    void SubscriptionReceivedHandle(Subscription subscription, object sender)
    {
        throw new NotImplementedException();
    }

    void SubscriptionOnlineHandle(Subscription subscription)
    {
        throw new NotImplementedException();
    }

    void SubscriptionFailedHandle(Subscription subscription, Exception ex, bool arg)
    {
        throw new NotImplementedException();
    }

    void SecurityReceivedHandle(Subscription subscription, Security sec)
    {
        dataRepo.SaveInstrument(new InstrumentTradeModel().Bind(sec));
        throw new NotImplementedException();
    }

    void PositionReceivedHandle(Subscription subscription, Position pos)
    {
        throw new NotImplementedException();
    }

    void PortfolioReceivedHandle(Subscription subscription, Portfolio port)
    {
        throw new NotImplementedException();
    }

    void ParentRemovedHandle(Ecng.Logging.ILogSource sender)
    {
        throw new NotImplementedException();
    }

    void OwnTradeReceivedHandle(Subscription subscription, MyTrade tr)
    {
        throw new NotImplementedException();
    }

    void OrderRegisterFailReceivedHandle(Subscription subscription, OrderFail orderF)
    {
        throw new NotImplementedException();
    }

    void OrderReceivedHandle(Subscription subscription, Order oreder)
    {
        throw new NotImplementedException();
    }

    void OrderLogReceivedHandle(Subscription subscription, StockSharp.Messages.IOrderLogMessage order)
    {
        throw new NotImplementedException();
    }

    void OrderEditFailReceivedHandle(Subscription subscription, OrderFail orderF)
    {
        throw new NotImplementedException();
    }

    void OrderCancelFailReceivedHandle(Subscription subscription, OrderFail orderF)
    {
        throw new NotImplementedException();
    }

    void OrderBookReceivedHandle(Subscription subscription, StockSharp.Messages.IOrderBookMessage orderBM)
    {
        throw new NotImplementedException();
    }

    void NewsReceivedHandle(Subscription subscription, News sender)
    {
        throw new NotImplementedException();
    }

    void NewMessageHandle(StockSharp.Messages.Message msg)
    {
        throw new NotImplementedException();
    }

    void MassOrderCancelFailed2Handle(long arg, Exception ex, DateTimeOffset dt)
    {
        throw new NotImplementedException();
    }

    void MassOrderCancelFailedHandle(long arg, Exception ex)
    {
        throw new NotImplementedException();
    }

    void MassOrderCanceled2Handle(long arg, DateTimeOffset dt)
    {
        throw new NotImplementedException();
    }

    void MassOrderCanceledHandle(long sender)
    {
        throw new NotImplementedException();
    }

    void LookupSecuritiesResultHandle(StockSharp.Messages.SecurityLookupMessage slm, IEnumerable<Security> securities, Exception ex)
    {
        foreach (Security sec in securities)
            dataRepo.SaveInstrument(new InstrumentTradeModel().Bind(sec));

        throw new NotImplementedException();
    }

    void LookupPortfoliosResultHandle(StockSharp.Messages.PortfolioLookupMessage arg1, IEnumerable<Portfolio> portfolios, Exception ex)
    {
        foreach (Portfolio port in portfolios)
            dataRepo.SavePortfolio(new PortfolioTradeModel().Bind(port));
        throw new NotImplementedException();
    }

    void LogHandle(Ecng.Logging.LogMessage senderLog)
    {
        throw new NotImplementedException();
    }

    void Level1ReceivedHandle(Subscription subscription, StockSharp.Messages.Level1ChangeMessage levelCh)
    {
        throw new NotImplementedException();
    }

    void ErrorHandle(Exception ex)
    {
        throw new NotImplementedException();
    }

    void DisposedHandle()
    {
        throw new NotImplementedException();
    }

    void DisconnectedExHandle(StockSharp.Messages.IMessageAdapter sender)
    {
        throw new NotImplementedException();
    }

    void DisconnectedHandle()
    {
        throw new NotImplementedException();
    }

    void DataTypeReceivedHandle(Subscription subscription, StockSharp.Messages.DataType argDt)
    {
        throw new NotImplementedException();
    }

    void CurrentTimeChangedHandle(TimeSpan sender)
    {
        throw new NotImplementedException();
    }

    void ConnectionRestoredHandle(StockSharp.Messages.IMessageAdapter sender)
    {
        throw new NotImplementedException();
    }

    void ConnectionLostHandle(StockSharp.Messages.IMessageAdapter sender)
    {
        throw new NotImplementedException();
    }

    void ConnectionErrorExHandle(StockSharp.Messages.IMessageAdapter sender, Exception ex)
    {
        throw new NotImplementedException();
    }

    void ConnectionErrorHandle(Exception ex)
    {
        throw new NotImplementedException();
    }

    void ConnectedExHandle(StockSharp.Messages.IMessageAdapter sender)
    {
        throw new NotImplementedException();
    }

    void ConnectedHandle()
    {
        throw new NotImplementedException();
    }

    void ChangePasswordResultHandle(long arg, Exception ex)
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