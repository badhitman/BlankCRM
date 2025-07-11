////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

public partial class CashFlowEditDialogComponent : BlazorBusyComponentBaseModel
{
    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public required InstrumentTradeStockSharpViewModel Instrument { get; set; }

    decimal? PaymentValue {  get; set; }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));

    void Cancel() => MudDialog.Cancel();
    private IEnumerable<CashFlowTypesEnum> _options = new HashSet<CashFlowTypesEnum>();
}