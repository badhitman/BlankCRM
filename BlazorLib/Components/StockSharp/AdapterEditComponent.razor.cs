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


    FixMessageAdapterModelDB? originAdapter;
    FixMessageAdapterModelDB? editAdapter;

    AdaptersTypesNames? AdapterType { get; set; }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Id < 1)
        {
            originAdapter = FixMessageAdapterModelDB.BuildEmpty();
            editAdapter = FixMessageAdapterModelDB.BuildEmpty();
            return;
        }

        await SetBusyAsync();
        TResponseModel<FixMessageAdapterModelDB[]> res = await SsRepo.AdaptersGetAsync([Id]);
        SnackbarRepo.ShowMessagesResponse(res.Messages);

        originAdapter = res.Response?.Single();
        editAdapter = GlobalTools.CreateDeepCopy(originAdapter);
        await SetBusyAsync(false);
    }
}