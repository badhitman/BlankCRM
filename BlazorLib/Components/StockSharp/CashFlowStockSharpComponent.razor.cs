////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using System.Collections.Generic;
using System.Globalization;

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

    /// <inheritdoc./>
    [CascadingParameter, EditorRequired]
    public required (DateTime? IssueDate, DateTime? MaturityDate) Period { get; set; }


    RegularCashFlowGenerateComponent? regularCashFlowGenerateRef;
    List<CashFlowViewModel> Elements = [];
    static readonly CultureInfo _en = CultureInfo.GetCultureInfo("en-US");

    MudTable<CashFlowViewModel>? _tableRef;

    DateTime? _startDate;
    DateTime? _endDate;

    decimal? _coupon = 0;
    decimal? _notional = 0;
    decimal? _couponRate = 0;

    CashFlowViewModel? selectedItem1 = null;
    CashFlowViewModel? elementBeforeEdit;
    int _initDeleteCashFlow;

    /// <inheritdoc/>
    public void UpdateState((DateTime? IssueDate, DateTime? MaturityDate) period, bool _set)
    {
        regularCashFlowGenerateRef?.UpdateState(period, _set);
    }

    async Task InitDeleteCashFlow(int _cashFlowId)
    {
        if (_initDeleteCashFlow == 0 || _initDeleteCashFlow != _cashFlowId)
        {
            _initDeleteCashFlow = _cashFlowId;
            return;
        }
        await SetBusyAsync();
        ResponseBaseModel res = await StockSharpRepo.CashFlowDelete(_initDeleteCashFlow);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        _initDeleteCashFlow = 0;
        await ReloadFlows();
        await SetBusyAsync(false);
    }

    async Task AddNewFlow()
    {
        _initDeleteCashFlow = 0;
        if (!_startDate.HasValue || !_endDate.HasValue || _startDate >= _endDate || !_couponRate.HasValue)
        {
            SnackBarRepo.Error("Invalid row: !_startDate.HasValue || !_endDate.HasValue || _startDate >= _endDate || !_couponRate.HasValue");
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel resC = await StockSharpRepo.CashFlowUpdateAsync(new()
        {
            InstrumentId = InstrumentId,
            CouponRate = _couponRate.Value,
            Notional = _notional ?? 0,
            Coupon = _coupon ?? 0,
            EndDate = _endDate.Value,
            StartDate = _startDate.Value,
        });
        SnackBarRepo.ShowMessagesResponse(resC.Messages);

        _startDate = null;
        _endDate = null;
        _couponRate = null;
        _notional = null;
        _coupon = null;

        await ReloadFlows();
        await SetBusyAsync(false);
    }

    void ActionUpdate()
    {
        InvokeAsync(ReloadFlows);
        if (_tableRef is not null)
            InvokeAsync(_tableRef.ReloadServerData);
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        await ReloadFlows();
        await SetBusyAsync(false);
    }

    void BackupItem(object element)
    {
        if (element is CashFlowViewModel cfm)
            elementBeforeEdit = GlobalTools.CreateDeepCopy(cfm);

        _initDeleteCashFlow = 0;
        StateHasChanged();
    }

    void ItemHasBeenCommitted(object element)
    {
        _initDeleteCashFlow = 0;
        if (element is CashFlowViewModel cfm)
        {
            CashFlowViewModel _cfm = GlobalTools.CreateDeepCopy(cfm)!;
            InvokeAsync(async () =>
            {
                await SetBusyAsync();
                ResponseBaseModel resC = await StockSharpRepo.CashFlowUpdateAsync(_cfm);
                SnackBarRepo.ShowMessagesResponse(resC.Messages);
                await ReloadFlows();
                await SetBusyAsync(false);
            });
        }
    }

    async Task ReloadFlows()
    {
        TResponseModel<List<CashFlowViewModel>> res = await StockSharpRepo.CashFlowList(InstrumentId);
        Elements = res.Response;
        Elements.Sort();

        if (_tableRef is not null)
            await _tableRef.ReloadServerData();
    }

    Task<TableData<CashFlowViewModel?>> ServerReload(TableState state, CancellationToken token)
    {
        return Task.FromResult(new TableData<CashFlowViewModel?>() { TotalItems = Elements.Count, Items = Elements.Skip(state.PageSize * state.Page).Take(state.PageSize) });
    }

    void ResetItemToOriginalValues(object element)
    {
        _initDeleteCashFlow = 0;
        if (elementBeforeEdit is null)
        {
            SnackBarRepo.Error("elementBeforeEdit is null");
            return;
        }

       ((CashFlowViewModel)element).StartDate = elementBeforeEdit.StartDate;
        ((CashFlowViewModel)element).EndDate = elementBeforeEdit.EndDate;
        ((CashFlowViewModel)element).Coupon = elementBeforeEdit.Coupon;
        ((CashFlowViewModel)element).CouponRate = elementBeforeEdit.CouponRate;
        ((CashFlowViewModel)element).Notional = elementBeforeEdit.Notional;
    }
}