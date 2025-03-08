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
    IClientHTTPRestService RemoteClient { get; set; } = default!;


    MetadataKladrModel? tmp, prod;

    async Task TransitData()
    {
        await SetBusy();
        ResponseBaseModel res = await RemoteClient.FlushTempKladr();
        await SetBusy(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await SetBusy();
        await Task.WhenAll([Task.Run(async () => tmp = await RemoteClient.GetMetadataKladr(new() { ForTemporary = true })),
            Task.Run(async () => prod = await RemoteClient.GetMetadataKladr(new() { ForTemporary = false }))]);
        await SetBusy(false);
    }
}