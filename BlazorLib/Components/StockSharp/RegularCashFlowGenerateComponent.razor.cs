////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using System.Threading.Tasks;

namespace BlazorLib.Components.StockSharp;

public partial class RegularCashFlowGenerateComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IManageStockSharpService ManageRepo { get; set; } = default!;


    /// <inheritdoc./>
    [Parameter,EditorRequired]
    public required Action UpdateHandle { get; set; }


    bool _visible;
    readonly DialogOptions _dialogOptions = new() { FullWidth = true };

    int fromDaysValue { get; set; } = 182;
    decimal notionalFirstValue { get; set; } = 1000;

    void OpenDialog() => _visible = true;

    async Task Submit()
    {
        CashFlowStockSharpRequestModel _req = new() { FromDays = fromDaysValue, NotionalFirst = notionalFirstValue };
        ResponseBaseModel res = await ManageRepo.GenerateRegularCashFlowsAsync(_req);
        _visible = false;
        UpdateHandle();
    }

    void Close()
    {

        _visible = false;
    }
}