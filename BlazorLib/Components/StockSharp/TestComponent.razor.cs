////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// TestComponent
/// </summary>
public partial class TestComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDataStockSharpService SsMainRepo { get; set; } = default!;

    [Inject]
    IDriverStockSharpService SsDrvRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public InstrumentTradeStockSharpViewModel? SetInstrument { get; set; }


    List<BoardStockSharpViewModel>? myBoards;
    BoardStockSharpModel? SelectedBoard { get; set; }

    OrderTypesEnum orderTypeCreate = OrderTypesEnum.Market;
    SidesEnum orderSideCreate = SidesEnum.Buy;

    List<PortfolioStockSharpViewModel>? myPortfolios;
    PortfolioStockSharpModel? SelectedPortfolio { get; set; }
    decimal? PriceNewOrder { get; set; }
    decimal? VolumeNewOrder { get; set; }

    List<InstrumentTradeStockSharpViewModel>? myInstruments;

    InstrumentTradeStockSharpViewModel? SelectedInstrument { get; set; }

    bool disposedValue;

    async Task NewOrder()
    {
        if (SelectedPortfolio is null)
        {
            SnackBarRepo.Error("Не выбран портфель");
            return;
        }

        if (SelectedInstrument is null)
        {
            SnackBarRepo.Error("Не выбран инструмент");
            return;
        }

        if (PriceNewOrder is null || PriceNewOrder <= 0)
        {
            SnackBarRepo.Error("Не указана стоимость");
            return;
        }

        if (VolumeNewOrder is null || VolumeNewOrder <= 0)
        {
            SnackBarRepo.Error("Не указан объём");
            return;
        }

        await SetBusyAsync();
        CreateOrderRequestModel req = new()
        {
            Instrument = SelectedInstrument,
            OrderType = orderTypeCreate,
            Portfolio = SelectedPortfolio,
            Price = PriceNewOrder.Value,
            Side = orderSideCreate,
            Volume = VolumeNewOrder.Value,
        };
        ResponseBaseModel res = await SsDrvRepo.OrderRegisterAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();

        await Task.WhenAll([
                Task.Run(async () => {
                    if(SetInstrument is null)
                    {
                        TResponseModel<List<BoardStockSharpViewModel>> resBoards = await SsMainRepo.GetBoardsAsync();
                        SnackBarRepo.ShowMessagesResponse(resBoards.Messages);
                        myBoards = resBoards.Response;
                    }
                    else
                    {
                        myBoards = [SetInstrument.Board];
                    }
                }),
                Task.Run(async () => {
                    if(SetInstrument is null)
                    {
                        TPaginationResponseModel<InstrumentTradeStockSharpViewModel> resInstruments = await SsMainRepo.InstrumentsSelectAsync(new() { PageSize = 100, StatesFilter = [ ObjectStatesEnum.IsFavorite, ObjectStatesEnum.Default ] });
                        myInstruments = resInstruments.Response;
                    }
                    else
                    {
                        myInstruments = [SetInstrument];
                    }
                }),
                Task.Run(async () => {
                    TResponseModel<List<PortfolioStockSharpViewModel>> resPortfolios = await SsMainRepo.GetPortfoliosAsync();
                    SnackBarRepo.ShowMessagesResponse(resPortfolios.Messages);
                    myPortfolios = resPortfolios.Response;
                }),
            ]);

        if (SetInstrument is not null)
        {
            SelectedBoard = SetInstrument.Board;
            SelectedInstrument = SetInstrument;
        }

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override void Dispose()
    {
        // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        Dispose(disposing: true);
        base.Dispose();
    }
}