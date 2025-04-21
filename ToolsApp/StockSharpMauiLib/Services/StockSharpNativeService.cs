////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Ecng.Common;
using SharedLib;
using StockSharp.BusinessEntities;
using StockSharp.Messages;
using System.Net;
using System.Security;

namespace StockSharpService;

/// <summary>
/// StockSharpNativeService
/// </summary>
public class StockSharpNativeService : IStockSharpDriverService
{
    const decimal _priceStep = 0.01m;
    const int _timeframe = 1;

    /// <inheritdoc/>
    public Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
    {
        StockSharp.Algo.Connector _connector = new();
        StockSharp.Quik.Lua.LuaFixMarketDataMessageAdapter luaFixMarketDataMessageAdapter = new(_connector.TransactionIdGenerator)
        {
            Address = "localhost:5001".To<EndPoint>(),
            //Login = "quik",
            //Password = "quik".To<SecureString>(),
            IsDemo = true,
        };
        StockSharp.Quik.Lua.LuaFixTransactionMessageAdapter luaFixTransactionMessageAdapter = new(_connector.TransactionIdGenerator)
        {
            Address = "localhost:5001".To<EndPoint>(),
            //Login = "quik",
            //Password = "quik".To<SecureString>(), 
            IsDemo = true,
        };
        _connector.Adapter.InnerAdapters.Add(luaFixMarketDataMessageAdapter);
        _connector.Adapter.InnerAdapters.Add(luaFixTransactionMessageAdapter);


        Security _security = new()
        {
            Id = "SBER@TQBR",
            PriceStep = _priceStep,
            Board = ExchangeBoard.Micex
        };


        // Создаем подписку на 5-минутные свечи
        Subscription subscription = new Subscription(TimeSpan.FromMinutes(_timeframe).TimeFrame(), _security)
        {
            // Настраиваем параметры подписки через свойство MarketData
            MarketData =
             {
                 // Запрашиваем данные за последние 30 дней
                 From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
                 // null означает, что подписка после получения истории перейдет в режим реального времени
                 To = null
             }
        };

        // Обработка полученных свечей
        _connector.CandleReceived += (sub, candle) =>
        {
            if (sub != subscription)
                return;

            // Обработка свечи
            Console.WriteLine($"Свеча: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");
        };

        // Обработка перехода подписки в онлайн-режим
        _connector.SubscriptionOnline += (sub) =>
        {
            if (sub != subscription)
                return;

            Console.WriteLine("Подписка перешла в режим реального времени");
        };

        // Обработка ошибок подписки
        _connector.SubscriptionFailed += (sub, error, isSubscribe) =>
        {
            if (sub != subscription)
                return;

            Console.WriteLine($"Ошибка подписки: {error}");
        };

        // Запуск подписки
        _connector.Subscribe(subscription);


        // Создаем подписку на стакан для выбранного инструмента
        Subscription depthSubscription = new Subscription(DataType.MarketDepth, _security);

        // Обработка полученных стаканов
        _connector.OrderBookReceived += (sub, depth) =>
        {
            if (sub != depthSubscription)
                return;

            // Обработка стакана
            Console.WriteLine($"Стакан: {depth.SecurityId}, Время: {depth.ServerTime}");
            //Console.WriteLine($"Покупки (Bids): {depth.Bids.Count}, Продажи (Asks): {depth.Asks.Count}");
        };

        // Запуск подписки
        _connector.Subscribe(depthSubscription);


        // Создаем подписку на тиковые сделки для выбранного инструмента
        Subscription tickSubscription = new Subscription(DataType.Ticks, _security);

        // Обработка полученных тиков
        _connector.TickTradeReceived += (sub, tick) =>
        {
            if (sub != tickSubscription)
                return;

            // Обработка тика
            Console.WriteLine($"Тик: {tick.SecurityId}, Время: {tick.ServerTime}, Цена: {tick.Price}, Объем: {tick.Volume}");
        };

        // Запуск подписки
        _connector.Subscribe(tickSubscription);


        // Подписка на 5-минутные свечи, которые будут построены из тиков
        Subscription candleSubscription = new Subscription(TimeSpan.FromMinutes(_timeframe).TimeFrame(), _security)
        {
            MarketData =
    {
        // Указываем режим построения и источник данных
        BuildMode = MarketDataBuildModes.Build,
        BuildFrom = DataType.Ticks,
        // Также можно включить построение профиля объема
        IsCalcVolumeProfile = true,
    }
        };

        _connector.Subscribe(candleSubscription);


        // Создание подписки на базовую информацию по инструменту
        Subscription level1Subscription = new Subscription(DataType.Level1, _security);

        // Обработка полученных данных Level1
        _connector.Level1Received += (sub, level1) =>
        {
            if (sub != level1Subscription)
                return;

            Console.WriteLine($"Level1: {level1.SecurityId}, Время: {level1.ServerTime}");

            // Вывод значений полей Level1
            foreach (var pair in level1.Changes)
            {
                Console.WriteLine($"Поле: {pair.Key}, Значение: {pair.Value}");
            }
        };

        // Запуск подписки
        _connector.Subscribe(level1Subscription);

        return Task.FromResult(ResponseBaseModel.CreateSuccess($"Ok - {nameof(StockSharpNativeService)}"));
    }
}