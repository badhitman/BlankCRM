////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using StockSharp.BusinessEntities;
using Newtonsoft.Json;
using StockSharp.Algo;
using SharedLib;

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
            await Task.Delay(200, stoppingToken);
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

    void SubscriptionFailedHandle(Subscription subscription, Exception ex, bool arg)
    {
        _logger.LogError(ex, $"Call > `{nameof(SubscriptionFailedHandle)}` [{nameof(arg)}:{arg}]: {JsonConvert.SerializeObject(subscription)}");
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

    void SecurityReceivedHandle(Subscription subscription, Security sec)
    {
        InstrumentTradeModel req = new InstrumentTradeModel().Bind(sec);
        dataRepo.SaveInstrument(req);
        eventTrans.InstrumentReceived(req);
    }

    void PortfolioReceivedHandle(Subscription subscription, Portfolio port)
    {
        throw new NotImplementedException();
    }

    void LookupSecuritiesResultHandle(StockSharp.Messages.SecurityLookupMessage slm, IEnumerable<Security> securities, Exception ex)
    {
        foreach (Security sec in securities)
            dataRepo.SaveInstrument(new InstrumentTradeModel().Bind(sec));

        _logger.LogError(ex, $"Call > `{nameof(LookupSecuritiesResultHandle)}`: {JsonConvert.SerializeObject(slm)}");
    }

    void LookupPortfoliosResultHandle(StockSharp.Messages.PortfolioLookupMessage portfolioLM, IEnumerable<Portfolio> portfolios, Exception ex)
    {
        foreach (Portfolio port in portfolios)
            dataRepo.SavePortfolio(new PortfolioTradeModel().Bind(port));

        _logger.LogError(ex, $"Call > `{nameof(LookupPortfoliosResultHandle)}`: {JsonConvert.SerializeObject(portfolioLM)}");
    }

    void BoardReceivedHandle(Subscription subscription, ExchangeBoard boardExchange)
    {
        _logger.LogWarning($"Call > `{nameof(BoardReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(boardExchange)}");
    }


    #region todo
    void TickTradeReceivedHandle(Subscription subscription, StockSharp.Messages.ITickTradeMessage msg)
    {
        _logger.LogWarning($"Call > `{nameof(TickTradeReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(msg)}");
    }
    void SubscriptionStoppedHandle(Subscription subscription, Exception ex)
    {
        _logger.LogError(ex, $"Call > `{nameof(SubscriptionStoppedHandle)}`: {JsonConvert.SerializeObject(subscription)}");
    }
    void SubscriptionStartedHandle(Subscription subscription)
    {
        _logger.LogWarning($"Call > `{nameof(SubscriptionStartedHandle)}`: {JsonConvert.SerializeObject(subscription)}");
    }
    void SubscriptionReceivedHandle(Subscription subscription, object sender)
    {
        _logger.LogWarning($"Call > `{nameof(SubscriptionReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(sender)}");
    }
    void SubscriptionOnlineHandle(Subscription subscription)
    {
        _logger.LogWarning($"Call > `{nameof(SubscriptionOnlineHandle)}`: {JsonConvert.SerializeObject(subscription)}");
    }
    void PositionReceivedHandle(Subscription subscription, Position pos)
    {
        _logger.LogWarning($"Call > `{nameof(PositionReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(pos)}");
    }
    void ParentRemovedHandle(Ecng.Logging.ILogSource sender)
    {
        _logger.LogWarning($"Call > `{nameof(ParentRemovedHandle)}`: {JsonConvert.SerializeObject(sender)}");
    }
    void OwnTradeReceivedHandle(Subscription subscription, MyTrade tr)
    {
        _logger.LogWarning($"Call > `{nameof(OwnTradeReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(tr)}");
    }
    void OrderRegisterFailReceivedHandle(Subscription subscription, OrderFail orderF)
    {
        _logger.LogWarning($"Call > `{nameof(OrderRegisterFailReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(orderF)}");
    }
    void OrderReceivedHandle(Subscription subscription, Order oreder)
    {
        _logger.LogWarning($"Call > `{nameof(OrderReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(oreder)}");
    }
    void OrderLogReceivedHandle(Subscription subscription, StockSharp.Messages.IOrderLogMessage order)
    {
        _logger.LogWarning($"Call > `{nameof(OrderLogReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(order)}");
    }
    void OrderEditFailReceivedHandle(Subscription subscription, OrderFail orderF)
    {
        _logger.LogWarning($"Call > `{nameof(OrderEditFailReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(orderF)}");
    }
    void OrderCancelFailReceivedHandle(Subscription subscription, OrderFail orderF)
    {
        _logger.LogWarning($"Call > `{nameof(OrderCancelFailReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(orderF)}");
    }
    void OrderBookReceivedHandle(Subscription subscription, StockSharp.Messages.IOrderBookMessage orderBM)
    {
        _logger.LogWarning($"Call > `{nameof(OrderBookReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(orderBM)}");
    }
    void NewsReceivedHandle(Subscription subscription, News sender)
    {
        _logger.LogWarning($"Call > `{nameof(NewsReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(sender)}");
    }
    void NewMessageHandle(StockSharp.Messages.Message msg)
    {
        _logger.LogWarning($"Call > `{nameof(NewMessageHandle)}`: {JsonConvert.SerializeObject(msg)}");
    }
    void MassOrderCancelFailed2Handle(long arg, Exception ex, DateTimeOffset dt)
    {
        _logger.LogError(ex, $"Call > `{nameof(MassOrderCancelFailed2Handle)}` [{nameof(arg)}:{arg}]: {dt}");
    }
    void MassOrderCancelFailedHandle(long arg, Exception ex)
    {
        _logger.LogError(ex, $"Call > `{nameof(MassOrderCancelFailedHandle)}` [{nameof(arg)}:{arg}]");
    }
    void MassOrderCanceled2Handle(long arg, DateTimeOffset dt)
    {
        _logger.LogWarning($"Call > `{nameof(MassOrderCanceled2Handle)}` [{nameof(arg)}:{arg}]: {dt}");
    }
    void MassOrderCanceledHandle(long sender)
    {
        _logger.LogWarning($"Call > `{nameof(MassOrderCanceledHandle)}`: {JsonConvert.SerializeObject(sender)}");
    }
    void LogHandle(Ecng.Logging.LogMessage senderLog)
    {
        _logger.LogWarning($"Call > `{nameof(LogHandle)}`: {senderLog}");
    }
    void Level1ReceivedHandle(Subscription subscription, StockSharp.Messages.Level1ChangeMessage levelCh)
    {
        _logger.LogWarning($"Call > `{nameof(Level1ReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(levelCh)}");
    }
    void ErrorHandle(Exception ex)
    {
        _logger.LogError(ex, $"Call > `{nameof(ErrorHandle)}`");
    }
    void DisposedHandle()
    {
        _logger.LogWarning($"Call > `{nameof(DisposedHandle)}`");
    }
    void DisconnectedExHandle(StockSharp.Messages.IMessageAdapter sender)
    {
        _logger.LogWarning($"Call > `{nameof(DisconnectedExHandle)}`: {JsonConvert.SerializeObject(sender)}");
    }
    void DisconnectedHandle()
    {
        _logger.LogWarning($"Call > `{nameof(DisconnectedHandle)}`");
    }
    void DataTypeReceivedHandle(Subscription subscription, StockSharp.Messages.DataType argDt)
    {
        _logger.LogWarning($"Call > `{nameof(DataTypeReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(argDt)}");
    }
    void CurrentTimeChangedHandle(TimeSpan sender)
    {
        _logger.LogWarning($"Call > `{nameof(CurrentTimeChangedHandle)}`: {JsonConvert.SerializeObject(sender)}");
    }
    void ConnectionRestoredHandle(StockSharp.Messages.IMessageAdapter sender)
    {
        _logger.LogWarning($"Call > `{nameof(ConnectionRestoredHandle)}`: {JsonConvert.SerializeObject(sender)}");
    }
    void ConnectionLostHandle(StockSharp.Messages.IMessageAdapter sender)
    {
        _logger.LogWarning($"Call > `{nameof(ConnectionLostHandle)}`: {JsonConvert.SerializeObject(sender)}");
    }
    void ConnectionErrorExHandle(StockSharp.Messages.IMessageAdapter sender, Exception ex)
    {
        _logger.LogError(ex, $"Call > `{nameof(ConnectionErrorExHandle)}`: {JsonConvert.SerializeObject(sender)}");
    }
    void ConnectionErrorHandle(Exception ex)
    {
        _logger.LogError(ex, $"Call > `{nameof(ConnectionErrorHandle)}`");
    }
    void ConnectedExHandle(StockSharp.Messages.IMessageAdapter sender)
    {
        _logger.LogWarning($"Call > `{nameof(ConnectedExHandle)}`: {JsonConvert.SerializeObject(sender)}");
    }
    void ConnectedHandle()
    {
        _logger.LogWarning($"Call > `{nameof(ConnectedHandle)}`");
    }
    void ChangePasswordResultHandle(long arg, Exception ex)
    {
        _logger.LogError(ex, $"Call > `{nameof(ChangePasswordResultHandle)}`: {arg}");
    }
    void CandleReceivedHandle(Subscription subscription, StockSharp.Messages.ICandleMessage candleMessage)
    {
        _logger.LogWarning($"Call > `{nameof(CandleReceivedHandle)}`: {JsonConvert.SerializeObject(subscription)}\n\n{JsonConvert.SerializeObject(candleMessage)}");
    }
    #endregion
}