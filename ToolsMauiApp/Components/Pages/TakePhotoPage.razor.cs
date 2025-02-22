using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ToolsMauiApp.Components.Pages;

/// <summary>
/// TakePhotoPage
/// </summary>
public partial class TakePhotoPage : ComponentBase, IAsyncDisposable
{
    [Inject]
    IJSRuntime _jsRuntime { get; set; } = default!;


    private IJSObjectReference? module;

    /// <inheritdoc/>
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/site.js");
            await module.InvokeVoidAsync("DOMCleanup.createObserver");
        }
    }

    private async void IncrementCount()
    {
        FileResult? photo = null;
        if (DeviceInfo.Current.Platform == DevicePlatform.Android || DeviceInfo.Current.Platform == DevicePlatform.iOS)
            photo = await MediaPicker.CapturePhotoAsync();
        if (photo == null)
            return;

        module ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/site.js");

        Stream stream = await photo.OpenReadAsync();
        DotNetStreamReference dotnetImageStream = new(stream);
        await module.InvokeVoidAsync("DOMCleanup.captureImage", "cleanupDiv", dotnetImageStream);
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        if (module is not null)

            return module.DisposeAsync();

        return ValueTask.CompletedTask;
    }
}
