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
    decimal? _valuePayment = 0;
    CashFlowTypesEnum _cashFlowType;
    DateTime? _datePayment;

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
        await SetBusyAsync(false);
    }

    async Task AddNewFlow()
    {
        _initDeleteCashFlow = 0;
        if (!_datePayment.HasValue || !_valuePayment.HasValue)
            return;

        await SetBusyAsync();
        ResponseBaseModel resC = await StockSharpRepo.CashFlowUpdateAsync(new()
        {
            CashFlowType = (int)_cashFlowType,
            InstrumentId = InstrumentId,
            PaymentDate = _datePayment.Value,
            PaymentValue = _valuePayment.Value,
        });
        SnackBarRepo.ShowMessagesResponse(resC.Messages);
        _datePayment = null;
        _valuePayment = 0;

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
        _initDeleteCashFlow = 0;
        if (element is CashFlowViewModel cfm)
            elementBeforeEdit = GlobalTools.CreateDeepCopy(cfm);
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

       ((CashFlowViewModel)element).PaymentDate = elementBeforeEdit.PaymentDate;
        ((CashFlowViewModel)element).PaymentValue = elementBeforeEdit.PaymentValue;
        ((CashFlowViewModel)element).CashFlowType = elementBeforeEdit.CashFlowType;
    }
}