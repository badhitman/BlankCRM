////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace ToolsMauiApp.Components;

/// <summary>
/// KladrAboutComponent
/// </summary>
public partial class KladrAboutComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IClientRestToolsService RemoteClient { get; set; } = default!;


    MetadataKladrModel? tmp, prod;

    async Task TransitData()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await RemoteClient.FlushTempKladrAsync();
        await SetBusyAsync(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await ReloadData();
    }

    async Task ClearTempTables()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await RemoteClient.ClearTempKladrAsync();
        await SetBusyAsync(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await ReloadData();
    }

    async Task ReloadData()
    {
        await SetBusyAsync();
        await Task.WhenAll([Task.Run(async () => tmp = await RemoteClient.GetMetadataKladrAsync(new() { ForTemporary = true })),
            Task.Run(async () => prod = await RemoteClient.GetMetadataKladrAsync(new() { ForTemporary = false }))]);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadData();
    }
}