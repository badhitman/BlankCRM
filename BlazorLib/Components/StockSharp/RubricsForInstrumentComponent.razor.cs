////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// RubricsForInstrumentComponent
/// </summary>
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

    UniversalBaseModel? _selectedRubric;
    UniversalBaseModel? SelectedRubric
    {
        get => _selectedRubric;
        set
        {
            //_selectedRubrics = null;
            _selectedRubric = value;
            InvokeAsync(SetRubricsForInstrument);
        }
    }

    IReadOnlyCollection<UniversalBaseModel>? _selectedRubrics;
    IReadOnlyCollection<UniversalBaseModel>? SelectedRubrics
    {
        get => _selectedRubrics;
        set
        {
            _selectedRubric = null;
            _selectedRubrics = value;
            InvokeAsync(SetRubricsForInstrument);
        }
    }

    async Task SetRubricsForInstrument()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await SsRepo.RubricsInstrumentUpdateAsync(new()
        {
            InstrumentId = Instrument.Id,
            RubricsIds = SelectedRubric is null ? null : [SelectedRubric.Id]
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await RubricsForInstrument();
        await SetBusyAsync(false);
    }

    void OpenDialog() => _visible = true;

    void Submit()
    {
        _dialogRef?.CloseAsync();
        _visible = false;
    }

    async Task RubricsForInstrument()
    {
        TResponseModel<List<UniversalBaseModel>> res = await SsRepo.GetRubricsForInstrumentAsync(Instrument.Id);
        _selectedRubrics = res.Response is null ? null : [.. res.Response];
        _selectedRubric = _selectedRubrics?.FirstOrDefault();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        RubricsAll = await RubricsRepo.RubricsChildListAsync(new());
        await RubricsForInstrument();
        await SetBusyAsync(false);
    }

    private readonly ElementComparer Comparer = new();
    class ElementComparer : IEqualityComparer<UniversalBaseModel>
    {
        public bool Equals(UniversalBaseModel? a, UniversalBaseModel? b) => a?.Id == b?.Id;
        public int GetHashCode(UniversalBaseModel x) => HashCode.Combine(x?.Id);
    }
}