////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

public partial class RegularCashFlowGenerateComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IManageStockSharpService ManageRepo { get; set; } = default!;


    /// <inheritdoc./>
    [Parameter, EditorRequired]
    public required Action UpdateHandle { get; set; }

    /// <inheritdoc./>
    [Parameter, EditorRequired]
    public required int InstrumentId { get; set; }

    /// <inheritdoc./>
    [Parameter, EditorRequired]
    public DateTime? IssueDate { get; set; }

    /// <inheritdoc./>
    [Parameter, EditorRequired]
    public DateTime? MaturityDate { get; set; }

    (DateTime? IssueDate, DateTime? MaturityDate) Period;

    bool _visible;
    readonly DialogOptions _dialogOptions = new() { FullWidth = true };

    int FromDaysValue { get; set; } = 182;
    decimal NotionalFirstValue { get; set; } = 1000;

    bool _holdInstrumentGen;
    /// <inheritdoc/>
    public void UpdateState((DateTime? IssueDate, DateTime? MaturityDate) period, bool _set)
    {
        Period = period;
        _holdInstrumentGen = _set;
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Period = (IssueDate, MaturityDate);
    }

    void OpenDialog() => _visible = true;

    async Task Submit()
    {
        CashFlowStockSharpRequestModel _req = new()
        {
            FromDays = FromDaysValue,
            NotionalFirst = NotionalFirstValue,
            InstrumentId = InstrumentId,
            IssueDate = IssueDate,
            MaturityDate = MaturityDate,
        };
        ResponseBaseModel res = await ManageRepo.GenerateRegularCashFlowsAsync(_req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        _visible = false;
        UpdateHandle();
    }

    void Close()
    {
        _visible = false;
    }
}