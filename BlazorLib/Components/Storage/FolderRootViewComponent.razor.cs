////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SharedLib;

namespace BlazorLib.Components.Storage;

/// <summary>
/// FolderRootViewComponent
/// </summary>
public partial class FolderRootViewComponent : BlazorBusyComponentBaseAuthModel
{
    /// <inheritdoc/>
    [Inject]
    protected IJSRuntime JsRuntimeRepo { get; set; } = default!;

    [Inject]
    IStorageTransmission StorageRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string FolderPath { get; set; }


    DirectoryReadResponseModel? DirectoryData;
    string? SelectedValue;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        TResponseModel<DirectoryReadResponseModel> res = await StorageRepo.GetDirectoryInfoAsync(new DirectoryReadRequestModel() { FolderPath = FolderPath });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        DirectoryData = res.Response;
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected async Task ClipboardCopyHandle(string textCopy)
    {
        await JsRuntimeRepo.InvokeVoidAsync("clipboardCopy.copyText", textCopy);
        SnackBarRepo.Info($"ѕуть к папке `{textCopy}` скопирован в буфер обмена");
    }
}