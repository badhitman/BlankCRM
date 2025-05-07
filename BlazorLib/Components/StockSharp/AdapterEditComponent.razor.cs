////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// AdapterEditComponent
/// </summary>
public partial class AdapterEditComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IManageStockSharpService SsRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required int Id { get; set; }


    FixMessageAdapterModelDB? currentAdapter;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if(Id < 1)
        {
            //currentAdapter = FixMessageAdapterModelDB.BuildEmpty();
            return;
        }

        await SetBusyAsync();
        TResponseModel<FixMessageAdapterModelDB[]> res = await SsRepo.AdaptersGetAsync([Id]);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        currentAdapter = res.Response?.Single();
        await SetBusyAsync(false);
    }
}