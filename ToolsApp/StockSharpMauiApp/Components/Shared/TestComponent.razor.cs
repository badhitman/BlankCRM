//using Ecng.Common;
//using MudBlazor;
//using StockSharp.BusinessEntities;
//using StockSharp.Messages;
//using System.Net;
//using StockSharp.Algo;
//using System.Security;
//using Ecng.ComponentModel;
using BlazorLib;
using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;
using SharedLib;

namespace StockSharpMauiApp.Components.Shared;

public partial class TestComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IStockSharpMainService ssMainRepo { get; set; } = default!;


    //List<Portfolio> myPortfolios = [];
    //Portfolio? selectedPortfolio;
    //public decimal DecimalValue { get; set; }
    //List<Security> mySecurities = [];

    //string? _selectedBoard;
    //string? selectedBoard
    //{
    //    get => _selectedBoard;
    //    set
    //    {
    //        _selectedBoard = value;
    //        selectedTool = null;
    //    }
    //}
    //EntryAltModel? selectedTool;


    ConcurrentDictionary<string, List<EntryAltModel>> mapSecurities = [];
    DateTime _lastUpdate = DateTime.UtcNow;
    //string? statusLoadText;

    //const decimal _priceStep = 0.01m;
    //const int _timeframe = 1;

    bool disposedValue;

    //StockSharp.Algo.Connector _connector = new();
    //StockSharp.Quik.Lua.LuaFixMarketDataMessageAdapter luaFixMarketDataMessageAdapter = default!;
    //StockSharp.Quik.Lua.LuaFixTransactionMessageAdapter luaFixTransactionMessageAdapter = default!;
    //Thread? myThread1;

    //string PortName = "S00+00002DFD";
    //Portfolio MyPortf = new();

    bool IncomingDataProgress => (DateTime.UtcNow - _lastUpdate).TotalMilliseconds < 200;

    Task NewOrder()
    {
        //if (selectedPortfolio is null)
        //{
        //    SnackbarRepo.Error("Не выбран портфель");
        //    return Task.CompletedTask;
        //}

        //if (selectedTool is null)
        //{
        //    SnackbarRepo.Error("Не выбран инструмент");
        //    return Task.CompletedTask;
        //}

        //if (DecimalValue <= 0)
        //{
        //    SnackbarRepo.Error("Не указана стоимость");
        //    return Task.CompletedTask;
        //}

        //Security? currentSec = mySecurities.FirstOrDefault(x => x.Board.Code == selectedBoard && x.Code == selectedTool.Id);
        //if (currentSec is null)
        //{
        //    SnackbarRepo.Error("Не найден инструмент");
        //    return Task.CompletedTask;
        //}

        //Order order = new()
        //{
        //    // устанавливается тип заявки, в данном примере лимитный
        //    Type = OrderTypes.Limit,
        //    // устанавливается портфель для исполнения заявки
        //    Portfolio = selectedPortfolio,
        //    // устанавливается объём заявки
        //    Volume = 1,
        //    // устанавливается цена заявки
        //    Price = DecimalValue,
        //    // устанавливается инструмент
        //    Security = currentSec,
        //    // устанавливается направление заявки, в данном примере покупка
        //    Side = Sides.Buy,
        //};
        ////Метод RegisterOrder отправляет заявку на сервер
        //_connector.RegisterOrder(order);
        return Task.CompletedTask;
    }

    protected override Task OnInitializedAsync()
    {
        //luaFixMarketDataMessageAdapter = new(_connector.TransactionIdGenerator)
        //{
        //    Address = "localhost:5001".To<EndPoint>(),
        //    //Login = "quik",
        //    //Password = "quik".To<SecureString>(),
        //    IsDemo = true,
        //};
        //luaFixTransactionMessageAdapter = new(_connector.TransactionIdGenerator)
        //{
        //    Address = "localhost:5001".To<EndPoint>(),
        //    //Login = "quik",
        //    //Password = "quik".To<SecureString>(),
        //    IsDemo = true,
        //};
        //_connector.Adapter.InnerAdapters.Add(luaFixMarketDataMessageAdapter);
        //_connector.Adapter.InnerAdapters.Add(luaFixTransactionMessageAdapter);

        /*
         // Создаем подписку на 5-минутные свечи
            Subscription subscription = new (DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
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
         */

        #region event`s
        //// Обработка полученных свечей
        //_connector.CandleReceived += (sub, candle)
        //    => SnackbarRepo.Info($"Свеча: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");

        //// Обработка перехода подписки в онлайн-режим
        //_connector.SubscriptionOnline += (sub) => SnackbarRepo.Info($"Подписка [{sub}] перешла в режим реального времени");
        //// Обработка ошибок подписки
        //_connector.SubscriptionFailed += (sub, error, isSubscribe) =>
        //{
        //    SnackbarRepo.Info($"Ошибка подписки: {error}");
        //};
        //_connector.NewsReceived += (Subscription subscription, News news) =>
        //{
        //    // Обрабатываем полученную новость
        //    SnackbarRepo.Info($"Новость #{news.Id}: {news.Source} [{news.Headline} {news.ServerTime}] {news.Url}");

        //    /*// Если есть текст новости
        //    if (!string.IsNullOrEmpty(news.Story))
        //        SnackbarRepo.Info($"Текст: {news.Story}");

        //    // Если новость связана с конкретными инструментами
        //    if (!string.IsNullOrWhiteSpace(news.Security.Id))
        //        SnackbarRepo.Info($"Инструмент: {news.Security.Id}");*/
        //};
        //// Обработка полученных стаканов
        //_connector.OrderBookReceived += (sub, depth) =>
        //{
        //    // Обработка стакана
        //    SnackbarRepo.Info($"Стакан: {depth.SecurityId}, Время: {depth.ServerTime}");
        //};

        ////Подписаться на событие появления новых портфелей
        //_connector.PortfolioReceived += (Sub, portfolio) =>
        //{
        //    //SnackbarRepo.Success($"Новый профиль: {Sub}\n{portfolio}");

        //    IEnumerable<Portfolio> newItems = _connector.Portfolios.Where(p => myPortfolios?.Any(sp => sp.Name == p.Name) != true);
        //    if (newItems.Any())
        //    {
        //        myPortfolios.AddRange(newItems);
        //        InvokeAsync(StateHasChanged);
        //    }
        //};

        //// Обработка полученных тиков
        //_connector.TickTradeReceived += (sub, tick) =>
        //{
        //    // Обработка тика
        //    SnackbarRepo.Info($"Тик: {tick.SecurityId}, Время: {tick.ServerTime}, Цена: {tick.Price}, Объем: {tick.Volume}");
        //};


        //// Обработка полученных данных Level1
        //_connector.Level1Received += (sub, level1) =>
        //{
        //    SnackbarRepo.Info($"Level1: {level1.SecurityId}, Время: {level1.ServerTime}");

        //    // Вывод значений полей Level1
        //    foreach (var pair in level1.Changes)
        //    {
        //        SnackbarRepo.Info($"Поле: {pair.Key}, Значение: {pair.Value}");
        //    }
        //};

        //_connector.SecurityReceived += (Subscription arg1, Security arg2) =>
        //{
        //    Security[] newItems = [.. _connector.Securities.Where(s => !mySecurities.Any(ss => ss.Name == s.Name))];
        //    if (newItems.Length != 0)
        //    {
        //        myThread1 = new(() =>
        //        {
        //            Thread.Sleep(205);
        //            InvokeAsync(StateHasChanged);
        //        });

        //        mySecurities.AddRange(newItems);

        //        foreach (IGrouping<string, Security> bg in newItems.GroupBy(x => x.Board.Code))
        //        {
        //            if (!mapSecurities.ContainsKey(bg.Key))
        //                mapSecurities.TryAdd(bg.Key, []);

        //            mapSecurities[bg.Key].AddRange(bg.Select(x => EntryAltModel.Build(x.Code, x.Name)));
        //        }
        //        _lastUpdate = DateTime.UtcNow;
        //        statusLoadText = $"{mapSecurities.Count} ({mapSecurities.Sum(x => x.Value.Count)})";

        //        InvokeAsync(StateHasChanged);
        //        myThread1.Start();
        //    }
        //};
        #endregion

        // _connector.Connect();

        return base.OnInitializedAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                //_connector.CancelOrders();
                // TODO: освободить управляемое состояние (управляемые объекты)
                // Или можно отписаться от всех подписок
                //foreach (Subscription? sub in _connector.Subscriptions)
                //{
                //    _connector.UnSubscribe(sub);
                //}
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

    public override void Dispose()
    {
        // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        Dispose(disposing: true);
        base.Dispose();
    }
}