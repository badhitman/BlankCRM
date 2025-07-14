////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

public partial class InstrumentEditComponent : BlazorBusyComponentBaseModel
{
    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public required InstrumentTradeStockSharpViewModel Instrument { get; set; }

    DateTime? IssueDate { get; set; }
    DateTime? MaturityDate { get; set; }
    IEnumerable<BondsTypesInstrumentsManualEnum> OptionsBondsTypes { get; set; } = new HashSet<BondsTypesInstrumentsManualEnum>();
    IEnumerable<TypesInstrumentsManualEnum> OptionsTypesInstruments { get; set; } = new HashSet<TypesInstrumentsManualEnum>();
    string? ISIN { get; set; }
    decimal CouponRate { get; set; }
    decimal LastFairPrice { get; set; }
    string? Comment { get; set; }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();
}