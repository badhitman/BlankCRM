////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <inheritdoc/>
public partial class InstrumentEditComponent : BlazorBusyComponentBaseModel
{
    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public required InstrumentTradeStockSharpViewModel Instrument { get; set; }


    DateTime? IssueDate { get; set; }
    DateTime? MaturityDate { get; set; }
    BondsTypesInstrumentsManualEnum BondTypeManual { get; set; }
    TypesInstrumentsManualEnum TypeInstrumentManual { get; set; }
    string? ISIN { get; set; }
    decimal CouponRate { get; set; }
    decimal LastFairPrice { get; set; }
    string? Comment { get; set; }

    void Reset()
    {
        IssueDate = Instrument.IssueDate;
        MaturityDate = Instrument.MaturityDate;
        BondTypeManual = (BondsTypesInstrumentsManualEnum)Instrument.BondTypeInstrumentManual;
        TypeInstrumentManual = (TypesInstrumentsManualEnum)Instrument.TypeInstrumentManual;
        ISIN = Instrument.ISIN;
        CouponRate = Instrument.CouponRate;
        LastFairPrice = Instrument.LastFairPrice;
        Comment = Instrument.Comment;
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        Reset();
        base.OnInitialized();
    }

    bool IsEdited =>
        IssueDate != Instrument.IssueDate ||
        MaturityDate != Instrument.MaturityDate ||
        BondTypeManual != (BondsTypesInstrumentsManualEnum)Instrument.BondTypeInstrumentManual ||
        TypeInstrumentManual != (TypesInstrumentsManualEnum)Instrument.TypeInstrumentManual ||
        ISIN != Instrument.ISIN ||
        CouponRate != Instrument.CouponRate ||
        LastFairPrice != Instrument.LastFairPrice ||
        Comment != Instrument.Comment;

    void Submit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();
}