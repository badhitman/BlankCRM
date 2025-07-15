////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// CashFlowStockSharpComponent
/// </summary>
public partial class CashFlowStockSharpComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDataStockSharpService StockSharpRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public int InstrumentId { get; set; }


    List<CashFlowViewModel> CashFlow = [];

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        TResponseModel<List<CashFlowViewModel>> res = await StockSharpRepo.CashFlowList(InstrumentId);
        CashFlow = res.Response;
    }
}