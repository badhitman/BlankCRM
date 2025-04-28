using BlazorLib;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace StockSharpMauiApp.Components.Shared;

public partial class TestComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IStockSharpDriverService ssMainRepo { get; set; } = default!;

    DateTime _lastUpdate = DateTime.UtcNow;


    List<BoardStockSharpModel>? myBoards;
    BoardStockSharpModel? selectedBoard { get; set; }


    List<PortfolioStockSharpModel>? myPortfolios;
    PortfolioStockSharpModel? selectedPortfolio { get; set; }


    bool disposedValue;

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

    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();

        TResponseModel<List<PortfolioStockSharpModel>> res = await ssMainRepo.GetPortfoliosAsync();
        SnackbarRepo.ShowMessagesResponse(res.Messages);

        TResponseModel<List<BoardStockSharpModel>> res2 = await ssMainRepo.GetBoardsAsync();

        myPortfolios = res.Response;
        await SetBusyAsync(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: освободить управляемое состояние (управляемые объекты)
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