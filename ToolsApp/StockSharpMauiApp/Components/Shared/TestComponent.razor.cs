////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace StockSharpMauiApp.Components.Shared;

public partial class TestComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IStockSharpDriverService SsMainRepo { get; set; } = default!;


    List<BoardStockSharpModel>? myBoards;
    BoardStockSharpModel? SelectedBoard { get; set; }


    List<PortfolioStockSharpModel>? myPortfolios;
    PortfolioStockSharpModel? SelectedPortfolio { get; set; }
    decimal? DecimalValue { get; set; }

    List<InstrumentTradeStockSharpModel>? myInstruments;
    InstrumentTradeStockSharpModel? SelectedInstrument { get; set; }

    bool disposedValue;

    Task NewOrder()
    {
        if (SelectedPortfolio is null)
        {
            SnackbarRepo.Error("Не выбран портфель");
            return Task.CompletedTask;
        }

        if (SelectedInstrument is null)
        {
            SnackbarRepo.Error("Не выбран инструмент");
            return Task.CompletedTask;
        }

        if (DecimalValue <= 0)
        {
            SnackbarRepo.Error("Не указана стоимость");
            return Task.CompletedTask;
        }

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

        return Task.CompletedTask;
    }

    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();

        TResponseModel<List<PortfolioStockSharpModel>> res = await SsMainRepo.GetPortfoliosAsync();
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        myPortfolios = res.Response;

        TResponseModel<List<BoardStockSharpModel>> res2 = await SsMainRepo.GetBoardsAsync();
        SnackbarRepo.ShowMessagesResponse(res2.Messages);
        myBoards = res2.Response;

        //TResponseModel<List<InstrumentTradeStockSharpModel>> res3 = await SsMainRepo.GetInstrumentsAsync();
        //SnackbarRepo.ShowMessagesResponse(res3.Messages);
        //myInstruments = res3.Response;

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