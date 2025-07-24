////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib.Components.Rubrics;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// InstrumentsForRubricComponent
/// </summary>
public partial class InstrumentsForRubricComponent : RubricNodeBodyComponent
{
    [Inject]
    IDataStockSharpService SsRepo { get; set; } = default!;

    [Inject]
    IDialogService DialogService { get; set; } = default!;


    List<InstrumentTradeStockSharpViewModel>? Instruments;

    async Task OpenDialog()
    {
        if (Instruments is null)
        {
            SnackBarRepo.Error("Instruments is null");
            return;
        }
        if (Node.Value is null)
        {
            SnackBarRepo.Error("Node.Value is null");
            return;
        }

        DialogParameters<InstrumentsForRubricDialogComponent> parameters = new()
        {
            { x => x.Instruments, Instruments },
            { x => x.OnRowClickHandle, OnRowClickAction },
            { x => x.RubricId, Node.Value.Id }
        };

        DialogOptions options = new() { MaxWidth = MaxWidth.Large, FullWidth = true, CloseOnEscapeKey = true };
        IDialogReference dialog = await DialogService.ShowAsync<InstrumentsForRubricDialogComponent>("Instrument`s for rubric", parameters, options);
        DialogResult? result = await dialog.Result;
        await InstrumentsForRubricUpdateAsync();
    }

    void OnRowClickAction()
    {
        InvokeAsync(InstrumentsForRubricUpdateAsync);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (Node.Value is null)
        {
            SnackBarRepo.Error("Node.Value is null");
            return;
        }
        await InstrumentsForRubricUpdateAsync();
    }

    async Task InstrumentsForRubricUpdateAsync()
    {
        await SetBusyAsync();
        Instruments = null;
        TResponseModel<List<InstrumentTradeStockSharpViewModel>> res = await SsRepo.GetInstrumentsForRubricAsync(Node.Value.Id);
        Instruments = res.Response;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }
}