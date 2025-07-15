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
    readonly CultureInfo _en = CultureInfo.GetCultureInfo("en-US");
    decimal? _valuePayment = 0;
    CashFlowTypesEnum _cashFlowType;
    DateTime? _datePayment;

    async Task AddNewFlow()
    {
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


    private CashFlowViewModel? selectedItem1 = null;
    private CashFlowViewModel? elementBeforeEdit;

    void BackupItem(object element)
    {
        if (element is CashFlowViewModel cfm)
            elementBeforeEdit = GlobalTools.CreateDeepCopy(cfm);
    }

    void ItemHasBeenCommitted(object element)
    {
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