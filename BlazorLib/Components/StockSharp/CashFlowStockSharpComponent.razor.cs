////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using System.Globalization;
using static MudBlazor.CategoryTypes;

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


    List<CashFlowViewModel> Elements = [];
    static readonly CultureInfo _en = CultureInfo.GetCultureInfo("en-US");

    DateTime? _startDate;
    DateTime? _endDate;

    decimal? _coupon = 0;
    decimal? _notional = 0;
    decimal? _couponRate = 0;

    CashFlowViewModel? selectedItem1 = null;
    CashFlowViewModel? elementBeforeEdit;
    int _initDeleteCashFlow;

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
        TResponseModel<List<CashFlowViewModel>> resC = await StockSharpRepo.CashFlowList(InstrumentId);
        Elements = resC.Response;
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
            Notional = _notional.HasValue ? _notional.Value : 0,
            Coupon = _coupon.HasValue ? _coupon.Value : 0,
            EndDate = _endDate.Value,
            StartDate = _startDate.Value,
        });
        SnackBarRepo.ShowMessagesResponse(resC.Messages);

        _startDate = null;
        _endDate = null;
        _couponRate = null;
        _notional = null;
        _coupon = null;

        TResponseModel<List<CashFlowViewModel>> res = await StockSharpRepo.CashFlowList(InstrumentId);
        Elements = res.Response;

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        TResponseModel<List<CashFlowViewModel>> res = await StockSharpRepo.CashFlowList(InstrumentId);
        Elements = res.Response;
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
                TResponseModel<List<CashFlowViewModel>> res = await StockSharpRepo.CashFlowList(InstrumentId);
                Elements = res.Response;
                await SetBusyAsync(false);
            });
        }
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