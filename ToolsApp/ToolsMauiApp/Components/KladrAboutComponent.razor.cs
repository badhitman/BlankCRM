////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib.Components.Kladr;
using BlazorLib;
using SharedLib;

namespace ToolsMauiApp.Components;

/// <summary>
/// KladrAboutComponent
/// </summary>
public partial class KladrAboutComponent : KladrAboutAbstractComponent
{
    [Inject]
    IClientRestToolsService RemoteClient { get; set; } = default!;


    /// <inheritdoc/>
    protected override async Task TransitData()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await RemoteClient.FlushTempKladrAsync();
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await ReloadData();
    }

    /// <inheritdoc/>
    protected override async Task ClearTempTables()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await RemoteClient.ClearTempKladrAsync();
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await ReloadData();
    }

    /// <inheritdoc/>
    protected override async Task ReloadData()
    {
        await SetBusyAsync();
        await Task.WhenAll([Task.Run(async () => tmp = await RemoteClient.GetMetadataKladrAsync(new() { ForTemporary = true })),
            Task.Run(async () => prod = await RemoteClient.GetMetadataKladrAsync(new() { ForTemporary = false }))]);
        await SetBusyAsync(false);
    }
}