////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using System.Globalization;
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


    DateTime? _issueDate;
    DateTime? IssueDate
    {
        get => _issueDate;
        set
        {
            _issueDate = value;
            cashFlowStockSharpRef?.UpdateState((IssueDate, MaturityDate), IsEdited);
        }
    }

    DateTime? _maturityDate;
    DateTime? MaturityDate
    {
        get => _maturityDate;
        set
        {
            _maturityDate = value;
            cashFlowStockSharpRef?.UpdateState((IssueDate, MaturityDate), IsEdited);
        }
    }

    BondsTypesInstrumentsManualEnum BondTypeManual { get; set; }
    TypesInstrumentsManualEnum TypeInstrumentManual { get; set; }
    string? ISIN { get; set; }
    string? Name { get; set; }

    decimal _couponRate = 1;
    decimal CouponRate
    {
        get => _couponRate;
        set
        {
            _couponRate = value;
            cashFlowStockSharpRef?.UpdateState((IssueDate, MaturityDate), IsEdited);
        }
    }

    decimal LastFairPrice { get; set; } = 9;
    string? Comment { get; set; }

    readonly CultureInfo _en = CultureInfo.GetCultureInfo("en-US");
    CashFlowStockSharpComponent? cashFlowStockSharpRef;

    /// <inheritdoc/>
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
    }

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
        base.OnInitialized();
        Reset();
    }

    bool IsEdited =>
        Name != Instrument.Name ||
        IssueDate != Instrument.IssueDate ||
        MaturityDate != Instrument.MaturityDate ||
        BondTypeManual != Instrument.BondTypeInstrumentManual ||
        TypeInstrumentManual != Instrument.TypeInstrumentManual ||
        ISIN != Instrument.ISIN ||
        CouponRate != Instrument.CouponRate ||
        LastFairPrice != Instrument.LastFairPrice ||
        Comment != Instrument.Comment;

    async Task Submit()
    {
        await SetBusyAsync();
        Instrument.IssueDate = IssueDate ?? default;
        Instrument.MaturityDate = MaturityDate ?? default;
        Instrument.BondTypeInstrumentManual = BondTypeManual;
        Instrument.TypeInstrumentManual = TypeInstrumentManual;
        Instrument.ISIN = ISIN;
        Instrument.CouponRate = CouponRate;
        Instrument.LastFairPrice = LastFairPrice;
        Instrument.Comment = Comment;
        Instrument.Name = Name;

        ResponseBaseModel res = await StockSharpDataRepo.UpdateInstrumentAsync(Instrument);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        MudDialog.Close(DialogResult.Ok(true));
    }

    void Cancel() => MudDialog.Cancel();
}