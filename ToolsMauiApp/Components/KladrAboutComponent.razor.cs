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
    IClientHTTPRestService remoteClient { get; set; } = default!;


    MetadataKladrModel? tmp, prod;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await SetBusy();
        await Task.WhenAll([Task.Run(async () => tmp = await remoteClient.GetMetadataKladr(new() { ForTemporary = true })),
            Task.Run(async () => prod = await remoteClient.GetMetadataKladr(new() { ForTemporary = false }))]);
        await SetBusy(false);
    }
}