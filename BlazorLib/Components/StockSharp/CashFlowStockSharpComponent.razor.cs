////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
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


    List<CashFlowViewModel> CashFlow = [];
    readonly CultureInfo _en = CultureInfo.GetCultureInfo("en-US");
    decimal? _valuePayment = 0;
    CashFlowTypesEnum _cashFlowType;
    DateOnly? _datePayment;

    async Task AddNewFlow()
    {
        if (!_datePayment.HasValue || !_valuePayment.HasValue)
            return;

        await SetBusyAsync();
        ResponseBaseModel resC = await StockSharpRepo.CashFlowUpdateAsync(new()
        {
            CashFlowType = (int)_cashFlowType,
            InstrumentId = InstrumentId,
            PaymentDate = _datePayment.Value.ToDateTime(new(0)),
            PaymentValue = _valuePayment.Value,
        });
        SnackBarRepo.ShowMessagesResponse(resC.Messages);
        _datePayment = null;
        _valuePayment = 0;

        TResponseModel<List<CashFlowViewModel>> res = await StockSharpRepo.CashFlowList(InstrumentId);
        CashFlow = res.Response;

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        TResponseModel<List<CashFlowViewModel>> res = await StockSharpRepo.CashFlowList(InstrumentId);
        CashFlow = res.Response;
        await SetBusyAsync(false);
    }
}