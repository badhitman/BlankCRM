using Ecng.Common;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorLib;
using StockSharp.BusinessEntities;
using StockSharp.Messages;
using System.Net;

namespace StockSharpMauiApp.Components.Shared;

public partial class TestComponent : IDisposable
{
    [Inject]
    ISnackbar SnackbarRepo { get; set; } = default!;

    const decimal _priceStep = 0.01m;
    const int _timeframe = 1;

    bool disposedValue;

    StockSharp.Algo.Connector _connector = new();
    StockSharp.BusinessEntities.Security _security = default!;
    StockSharp.Quik.Lua.LuaFixMarketDataMessageAdapter luaFixMarketDataMessageAdapter = default!;
    StockSharp.Quik.Lua.LuaFixTransactionMessageAdapter luaFixTransactionMessageAdapter = default!;

    Subscription subscription = default!;
    Subscription depthSubscription = default!;
    Subscription tickSubscription = default!;
    Subscription candleSubscription = default!;
    Subscription level1Subscription = default!;
    Subscription newsSubscription = default!;

    protected override Task OnInitializedAsync()
    {
        luaFixMarketDataMessageAdapter = new(_connector.TransactionIdGenerator)
        {
            Address = "localhost:5001".To<EndPoint>(),
            //Login = "quik",
            //Password = "quik".To<SecureString>(),
            IsDemo = true,
        };
        luaFixTransactionMessageAdapter = new(_connector.TransactionIdGenerator)
        {
            Address = "localhost:5001".To<EndPoint>(),
            //Login = "quik",
            //Password = "quik".To<SecureString>(),
            IsDemo = true,
        };
        _connector.Adapter.InnerAdapters.Add(luaFixMarketDataMessageAdapter);
        _connector.Adapter.InnerAdapters.Add(luaFixTransactionMessageAdapter);


        _security = new()
        {
            Id = "SBER@TQBR",
            PriceStep = _priceStep,
            Board = ExchangeBoard.Micex
        };


        // Создаем подписку на 5-минутные свечи
        subscription = new(TimeSpan.FromMinutes(_timeframe).TimeFrame(), _security)
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
            SnackbarRepo.Info($"Свеча: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");
        };

        // Обработка перехода подписки в онлайн-режим
        _connector.SubscriptionOnline += (sub) =>
        {
            if (sub != subscription)
                return;

            SnackbarRepo.Info("Подписка перешла в режим реального времени");
        };

        // Обработка ошибок подписки
        _connector.SubscriptionFailed += (sub, error, isSubscribe) =>
        {
            if (sub != subscription)
                return;

            SnackbarRepo.Info($"Ошибка подписки: {error}");
        };

        // Запуск подписки
        _connector.Subscribe(subscription);

        // Создаем подписку на новости
        newsSubscription = new(DataType.News);

        _connector.NewsReceived += (Subscription subscription, News news) =>
        {
            if (subscription != newsSubscription)
                return;

            // Обрабатываем полученную новость
            SnackbarRepo.Info($"Новость: {news.Id}");
            SnackbarRepo.Info($"Заголовок: {news.Headline}");
            SnackbarRepo.Info($"Источник: {news.Source}");
            SnackbarRepo.Info($"Время: {news.ServerTime}");
            SnackbarRepo.Info($"Ссылка: {news.Url}");

            // Если есть текст новости
            if (!string.IsNullOrEmpty(news.Story))
                SnackbarRepo.Info($"Текст: {news.Story}");

            // Если новость связана с конкретными инструментами
            if (!string.IsNullOrWhiteSpace(news.Security.Id))
                SnackbarRepo.Info($"Инструмент: {news.Security.Id}");
        };

        // Создаем подписку на стакан для выбранного инструмента
        depthSubscription = new(DataType.MarketDepth, _security);

        // Обработка полученных стаканов
        _connector.OrderBookReceived += (sub, depth) =>
        {
            if (sub != depthSubscription)
                return;

            // Обработка стакана
            SnackbarRepo.Info($"Стакан: {depth.SecurityId}, Время: {depth.ServerTime}");
            //Console.WriteLine($"Покупки (Bids): {depth.Bids.Count}, Продажи (Asks): {depth.Asks.Count}");
        };

        // Запуск подписки
        _connector.Subscribe(depthSubscription);


        // Создаем подписку на тиковые сделки для выбранного инструмента
        tickSubscription = new(DataType.Ticks, _security);

        // Обработка полученных тиков
        _connector.TickTradeReceived += (sub, tick) =>
        {
            if (sub != tickSubscription)
                return;

            // Обработка тика
            SnackbarRepo.Info($"Тик: {tick.SecurityId}, Время: {tick.ServerTime}, Цена: {tick.Price}, Объем: {tick.Volume}");
        };

        // Запуск подписки
        _connector.Subscribe(tickSubscription);


        // Подписка на 5-минутные свечи, которые будут построены из тиков
        candleSubscription = new(TimeSpan.FromMinutes(_timeframe).TimeFrame(), _security)
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
        level1Subscription = new(DataType.Level1, _security);

        // Обработка полученных данных Level1
        _connector.Level1Received += (sub, level1) =>
        {
            if (sub != level1Subscription)
                return;

            SnackbarRepo.Info($"Level1: {level1.SecurityId}, Время: {level1.ServerTime}");

            // Вывод значений полей Level1
            foreach (var pair in level1.Changes)
            {
                SnackbarRepo.Info($"Поле: {pair.Key}, Значение: {pair.Value}");
            }
        };

        // Запуск подписки
        _connector.Subscribe(level1Subscription);


        return base.OnInitializedAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: освободить управляемое состояние (управляемые объекты)
                // Отписка от конкретной подписки
                _connector.UnSubscribe(subscription);

                // Или можно отписаться от всех подписок
                foreach (Subscription? sub in _connector.Subscriptions)
                {
                    _connector.UnSubscribe(sub);
                }
            }

            // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
            // TODO: установить значение NULL для больших полей
            disposedValue = true;
        }
    }

    // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
    // ~TestComponent()
    // {
    //     // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}