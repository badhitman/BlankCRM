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
    [Inject]
    IDataStockSharpService StockSharpDataRepo { get; set; } = default!;

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
    string? Name { get; set; }
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
        Name = Instrument.Name;
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

    async Task Submit()
    {
        await SetBusyAsync();
        Instrument.IssueDate = IssueDate ?? default;
        Instrument.MaturityDate = MaturityDate ?? default;
        Instrument.BondTypeInstrumentManual = (int)BondTypeManual;
        Instrument.TypeInstrumentManual = (int)TypeInstrumentManual;
        Instrument.ISIN = ISIN;
        Instrument.CouponRate = CouponRate;
        Instrument.LastFairPrice = LastFairPrice;
        Instrument.Comment = Comment;
        Instrument.Name = Name;

        ResponseBaseModel res =  await StockSharpDataRepo.UpdateInstrumentAsync(Instrument);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        MudDialog.Close(DialogResult.Ok(true));
    }

    void Cancel() => MudDialog.Cancel();
}