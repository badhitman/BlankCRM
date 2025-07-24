////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

public partial class RubricsForInstrumentComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDataStockSharpService SsRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission RubricsRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required InstrumentTradeStockSharpViewModel Instrument { get; set; }


    List<UniversalBaseModel>? RubricsAll;
    MudDialog? _dialogRef;
    bool _visible;
    readonly DialogOptions _dialogOptions = new() { FullWidth = true };

    IReadOnlyCollection<UniversalBaseModel>? _selectedRubrics;
    IReadOnlyCollection<UniversalBaseModel>? SelectedRubrics
    {
        get => _selectedRubrics;
        set
        {
            _selectedRubrics = value;
            InvokeAsync(SetRubricsForInstrument);
        }
    }

    async Task SetRubricsForInstrument()
    {
        if (SelectedRubrics is null)
        {
            SnackBarRepo.Error("SelectedRubrics is null");
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel res = await SsRepo.RubricsInstrumentUpdateAsync(new() { InstrumentId = Instrument.Id, RubricsIds = [.. SelectedRubrics.Select(x => x.Id)] });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }

    void OpenDialog() => _visible = true;

    void Submit()
    {
        _dialogRef?.CloseAsync();
        _visible = false;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        RubricsAll = await RubricsRepo.RubricsListAsync(new());
        TResponseModel<List<UniversalBaseModel>> res = await SsRepo.GetRubricsForInstrumentAsync(Instrument.Id);
        _selectedRubrics = [.. res.Response];
        await SetBusyAsync(false);
    }

    private readonly ElementComparer Comparer = new();
    class ElementComparer : IEqualityComparer<UniversalBaseModel>
    {
        public bool Equals(UniversalBaseModel? a, UniversalBaseModel? b) => a?.Id == b?.Id;
        public int GetHashCode(UniversalBaseModel x) => HashCode.Combine(x?.Id);
    }
}