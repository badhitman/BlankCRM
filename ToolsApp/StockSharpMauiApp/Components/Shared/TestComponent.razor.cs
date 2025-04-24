using Ecng.Common;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BlazorLib;
using StockSharp.BusinessEntities;
using StockSharp.Messages;
using System.Net;
using StockSharp.Algo;
using System.Security;
using System.Collections.Concurrent;
using SharedLib;
using Ecng.ComponentModel;

namespace StockSharpMauiApp.Components.Shared;

public partial class TestComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ISnackbar SnackbarRepo { get; set; } = default!;


    List<Portfolio> myPortfolios = [];
    Portfolio? selectedPortfolio;
    public decimal DecimalValue { get; set; }
    List<Security> mySecurities = [];

    string? _selectedBoard;
    string? selectedBoard
    {
        get => _selectedBoard;
        set
        {
            _selectedBoard = value;
            selectedTool = null;
        }
    }
    EntryAltModel? selectedTool;


    ConcurrentDictionary<string, List<EntryAltModel>> mapSecurities = [];
    DateTime _lastUpdate = DateTime.UtcNow;
    string? statusLoadText;

    const decimal _priceStep = 0.01m;
    const int _timeframe = 1;

    bool disposedValue;

    StockSharp.Algo.Connector _connector = new();
    StockSharp.Quik.Lua.LuaFixMarketDataMessageAdapter luaFixMarketDataMessageAdapter = default!;
    StockSharp.Quik.Lua.LuaFixTransactionMessageAdapter luaFixTransactionMessageAdapter = default!;
    Thread? myThread1;

    //string PortName = "S00+00002DFD";
    //Portfolio MyPortf = new();

    bool IncomingDataProgress => (DateTime.UtcNow - _lastUpdate).TotalMilliseconds < 200;

    Task NewOrder()
    {
        if (selectedPortfolio is null)
        {
            SnackbarRepo.Error("Не выбран портфель");
            return Task.CompletedTask;
        }

        if (selectedTool is null)
        {
            SnackbarRepo.Error("Не выбран инструмент");
            return Task.CompletedTask;
        }

        if (DecimalValue <= 0)
        {
            SnackbarRepo.Error("Не указана стоимость");
            return Task.CompletedTask;
        }

        Security? currentSec = mySecurities.FirstOrDefault(x => x.Board.Code == selectedBoard && x.Code == selectedTool.Id);
        if (currentSec is null)
        {
            SnackbarRepo.Error("Не найден инструмент");
            return Task.CompletedTask;
        }

        Order order = new()
        {
            // устанавливается тип заявки, в данном примере лимитный
            Type = OrderTypes.Limit,
            // устанавливается портфель для исполнения заявки
            Portfolio = selectedPortfolio,
            // устанавливается объём заявки
            Volume = 1,
            // устанавливается цена заявки
            Price = DecimalValue,
            // устанавливается инструмент
            Security = currentSec,
            // устанавливается направление заявки, в данном примере покупка
            Side = Sides.Buy,
        };
        //Метод RegisterOrder отправляет заявку на сервер
        _connector.RegisterOrder(order);
        return Task.CompletedTask;
    }

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

        #region event`s
        // Обработка полученных свечей
        _connector.CandleReceived += (sub, candle)
            => SnackbarRepo.Info($"Свеча: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");

        // Обработка перехода подписки в онлайн-режим
        _connector.SubscriptionOnline += (sub) => SnackbarRepo.Info($"Подписка [{sub}] перешла в режим реального времени");
        // Обработка ошибок подписки
        _connector.SubscriptionFailed += (sub, error, isSubscribe) =>
        {
            SnackbarRepo.Info($"Ошибка подписки: {error}");
        };
        _connector.NewsReceived += (Subscription subscription, News news) =>
        {
            // Обрабатываем полученную новость
            SnackbarRepo.Info($"Новость #{news.Id}: {news.Source} [{news.Headline} {news.ServerTime}] {news.Url}");

            /*// Если есть текст новости
            if (!string.IsNullOrEmpty(news.Story))
                SnackbarRepo.Info($"Текст: {news.Story}");

            // Если новость связана с конкретными инструментами
            if (!string.IsNullOrWhiteSpace(news.Security.Id))
                SnackbarRepo.Info($"Инструмент: {news.Security.Id}");*/
        };
        // Обработка полученных стаканов
        _connector.OrderBookReceived += (sub, depth) =>
        {
            // Обработка стакана
            SnackbarRepo.Info($"Стакан: {depth.SecurityId}, Время: {depth.ServerTime}");
        };

        //Подписаться на событие появления новых портфелей
        _connector.PortfolioReceived += (Sub, portfolio) =>
        {
            //SnackbarRepo.Success($"Новый профиль: {Sub}\n{portfolio}");

            IEnumerable<Portfolio> newItems = _connector.Portfolios.Where(p => myPortfolios?.Any(sp => sp.Name == p.Name) != true);
            if (newItems.Any())
            {
                myPortfolios.AddRange(newItems);
                InvokeAsync(StateHasChanged);
            }
        };

        // Обработка полученных тиков
        _connector.TickTradeReceived += (sub, tick) =>
        {
            // Обработка тика
            SnackbarRepo.Info($"Тик: {tick.SecurityId}, Время: {tick.ServerTime}, Цена: {tick.Price}, Объем: {tick.Volume}");
        };


        // Обработка полученных данных Level1
        _connector.Level1Received += (sub, level1) =>
        {
            SnackbarRepo.Info($"Level1: {level1.SecurityId}, Время: {level1.ServerTime}");

            // Вывод значений полей Level1
            foreach (var pair in level1.Changes)
            {
                SnackbarRepo.Info($"Поле: {pair.Key}, Значение: {pair.Value}");
            }
        };

        _connector.Connected += _connector_Connected;
        _connector.ConnectedEx += _connector_ConnectedEx;
        _connector.ConnectionError += _connector_ConnectionError;
        _connector.ConnectionErrorEx += _connector_ConnectionErrorEx;
        _connector.ConnectionLost += _connector_ConnectionLost;
        _connector.SecurityReceived += _connector_SecurityReceived;

        _connector.BoardReceived += _connector_BoardReceived;
        _connector.DataTypeReceived += _connector_DataTypeReceived;
        _connector.Disconnected += _connector_Disconnected;
        _connector.DisconnectedEx += _connector_DisconnectedEx;
        _connector.Error += _connector_Error;
        //_connector.Log += _connector_Log;
        _connector.OrderCancelFailReceived += _connector_OrderCancelFailReceived;
        _connector.OrderLogReceived += _connector_OrderLogReceived;
        _connector.OrderReceived += _connector_OrderReceived;
        _connector.OrderRegisterFailReceived += _connector_OrderRegisterFailReceived;
        _connector.OwnTradeReceived += _connector_OwnTradeReceived;

        #endregion

        _connector.Connect();

        /*Order order = new()
        {
            // устанавливается тип заявки, в данном примере лимитный
            Type = OrderTypes.Limit,
            // устанавливается портфель для исполнения заявки //NC0011100000
            Portfolio = new Portfolio() {  ClientCode = "10028", Name = "NL0011100043", Security = _security },
            // устанавливается объём заявки
            Volume = 1,
            // устанавливается цена заявки
            Price = 3,
            // устанавливается инструмент
            Security = _security,
            // устанавливается направление заявки, в данном примере покупка
            Side = Sides.Buy,
        };
        //Метод RegisterOrder отправляет заявку на сервер
        _connector.RegisterOrder(order);*/

        return base.OnInitializedAsync();
    }

    private void _connector_SubscriptionFailed(Subscription arg1, Exception arg2, bool arg3)
    {
        SnackbarRepo.Error($"SubscriptionFailed - {arg1}: {arg2} /{arg3}");
    }

    private void _connector_OwnTradeReceived(Subscription arg1, MyTrade arg2)
    {
        SnackbarRepo.Info($"OwnTradeReceived - {arg1}: {arg2}");
    }

    private void _connector_OrderRegisterFailReceived(Subscription arg1, OrderFail arg2)
    {
        SnackbarRepo.Error($"OrderRegisterFailReceived: {arg1} - {arg2}");
    }

    private void _connector_OrderReceived(Subscription arg1, Order arg2)
    {
        SnackbarRepo.Info($"OrderReceived - {arg1}: {arg2}");
    }

    private void _connector_OrderLogReceived(Subscription arg1, IOrderLogMessage arg2)
    {
        SnackbarRepo.Info($"OrderLogReceived: {arg1} - {arg2}");
    }

    private void _connector_OrderCancelFailReceived(Subscription arg1, OrderFail arg2)
    {
        SnackbarRepo.Error($"OrderCancelFailReceived: {arg1} - {arg2}");
    }

    private void _connector_OrderCancelFailed(OrderFail obj)
    {
        SnackbarRepo.Error($"OrderCancelFailed - {obj}");
    }

    private void _connector_OrderBookReceived(Subscription arg1, IOrderBookMessage arg2)
    {
        SnackbarRepo.Info($"OrderBookReceived - {arg1}: {arg2}");
    }

    private void _connector_Log(Ecng.Logging.LogMessage obj)
    {
        SnackbarRepo.Info($"Log - {obj}");
    }

    private void _connector_Level1Received(Subscription arg1, Level1ChangeMessage arg2)
    {
        SnackbarRepo.Info($"Level1Received - {arg1}: {arg2}");
    }

    private void _connector_Error(Exception obj)
    {
        SnackbarRepo.Error($"Error - {obj.Message}");
    }

    private void _connector_DisconnectedEx(IMessageAdapter obj)
    {
        SnackbarRepo.Warn($"DisconnectedEx - {obj}");
    }

    private void _connector_Disconnected()
    {
        SnackbarRepo.Warn($"Disconnected");
    }

    private void _connector_DataTypeReceived(Subscription arg1, DataType arg2)
    {
        SnackbarRepo.Info($"DataTypeReceived - {arg1}: {arg2}");
    }

    private void _connector_BoardReceived(Subscription arg1, ExchangeBoard arg2)
    {
        SnackbarRepo.Info($"BoardReceived - {arg1}: {arg2}");
    }

    private void _connector_SecurityReceived(Subscription arg1, Security arg2)
    {
        //SnackbarRepo.Warn($"SecurityReceived ({arg1}) - {arg2}");

        Security[] newItems = [.. _connector.Securities.Where(s => !mySecurities.Any(ss => ss.Name == s.Name))];
        if (newItems.Length != 0)
        {
            myThread1 = new(() =>
            {
                Thread.Sleep(205);
                InvokeAsync(StateHasChanged);
            });

            mySecurities.AddRange(newItems);

            foreach (IGrouping<string, Security> bg in newItems.GroupBy(x => x.Board.Code))
            {
                if (!mapSecurities.ContainsKey(bg.Key))
                    mapSecurities.TryAdd(bg.Key, []);

                mapSecurities[bg.Key].AddRange(bg.Select(x => EntryAltModel.Build(x.Code, x.Name)));
            }
            _lastUpdate = DateTime.UtcNow;
            statusLoadText = $"{mapSecurities.Count} ({mapSecurities.Sum(x => x.Value.Count)})";

            InvokeAsync(StateHasChanged);
            myThread1.Start();
        }
    }

    private void _connector_ConnectionLost(IMessageAdapter adapter)
    {
        SnackbarRepo.Warn($"ConnectionLost - {adapter}");
    }

    private void _connector_ConnectionErrorEx(IMessageAdapter adapter, Exception exception)
    {
        SnackbarRepo.Error($"ConnectionErrorEx - {adapter}: {exception.Message}");
    }

    private void _connector_ConnectionError(Exception exception)
    {
        SnackbarRepo.Error($"ConnectionError: {exception}");
    }

    private void _connector_ConnectedEx(IMessageAdapter adapter)
    {
        SnackbarRepo.Info($"ConnectedEx: {adapter}");
    }

    private void _connector_Connected()
    {
        SnackbarRepo.Success($"Connected");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _connector.CancelOrders();
                // TODO: освободить управляемое состояние (управляемые объекты)
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

    public override void Dispose()
    {
        // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        Dispose(disposing: true);
        base.Dispose();
    }
}