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
    IParametersStorageTransmission StorageRepo { get; set; } = default!;

    [Inject]
    IDataStockSharpService DataRepo { get; set; } = default!;

    [Inject]
    IDriverStockSharpService DriverRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required InstrumentTradeStockSharpViewModel SetInstrument { get; set; }


    //OrderTypesEnum orderTypeCreate = OrderTypesEnum.Market;
    OrderTypesEnum _selectedOrderType;
    OrderTypesEnum SelectedOrderType
    {
        get => _selectedOrderType;
        set
        {
            _selectedOrderType = value;
            InvokeAsync(async () => { await StorageRepo.SaveParameterAsync(value, GlobalStaticCloudStorageMetadata.DashboardTradeOrderType, true); });
        }
    }

    SidesEnum orderSideCreate = SidesEnum.Buy;

    List<PortfolioStockSharpViewModel>? myPortfolios;
    int _selectedPortfolioId;
    int SelectedPortfolioId
    {
        get => _selectedPortfolioId;
        set
        {
            _selectedPortfolioId = value;
            InvokeAsync(async () => { await StorageRepo.SaveParameterAsync(value, GlobalStaticCloudStorageMetadata.DashboardTradePortfolio, true); });
        }
    }

    decimal? PriceNewOrder { get; set; }
    decimal? VolumeNewOrder { get; set; }

    bool disposedValue;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();

        await Task.WhenAll([
                Task.Run(async () => {
                TResponseModel<int> tradePortfolio = await StorageRepo.ReadParameterAsync<int>(GlobalStaticCloudStorageMetadata.DashboardTradePortfolio);
                if(tradePortfolio.Success() && tradePortfolio.Response != 0)
                    _selectedPortfolioId = tradePortfolio.Response;
                }),
                Task.Run(async () => {
                    TResponseModel<OrderTypesEnum?> orderType = await StorageRepo.ReadParameterAsync<OrderTypesEnum?>(GlobalStaticCloudStorageMetadata.DashboardTradeOrderType);
                    if(orderType.Success() && orderType.Response is not null)
                        _selectedOrderType = orderType.Response.Value;
                }),
                Task.Run(async () => {
                    TResponseModel<List<PortfolioStockSharpViewModel>> resPortfolios = await DataRepo.GetPortfoliosAsync();
                    SnackBarRepo.ShowMessagesResponse(resPortfolios.Messages);
                    myPortfolios = resPortfolios.Response;
                }),
            ]);

        if (_selectedPortfolioId == 0 && myPortfolios is not null && myPortfolios.Count != 0)
            SelectedPortfolioId = myPortfolios.First().Id;

        await SetBusyAsync(false);
    }

    async Task NewOrder()
    {
        if (SelectedPortfolioId <= 0)
        {
            SnackBarRepo.Error("Не выбран портфель");
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
            InstrumentId = SetInstrument.Id,
            OrderType = SelectedOrderType,
            PortfolioId = SelectedPortfolioId,
            Price = PriceNewOrder.Value,
            Side = orderSideCreate,
            Volume = VolumeNewOrder.Value,
        };
        ResponseBaseModel res = await DriverRepo.OrderRegisterAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
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