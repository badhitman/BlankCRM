////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.FileView;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
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

    [Inject]
    IDialogService DialogService { get; set; } = default!;


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

    Task OpenDialogAsync(DirectoryItemModel _fiElement)
    {
        DialogParameters<FileViewDialogComponent> parameters = new() { { x => x.DirectoryItem, _fiElement } };
        DialogOptions options = new()
        {
            CloseOnEscapeKey = true,
            BackdropClick = true,
            CloseButton = true,
            FullScreen = true,
            FullWidth = true,
        };

        return DialogService.ShowAsync<FileViewDialogComponent>($"View file data", parameters, options);
    }

    /// <inheritdoc/>
    protected async Task ClipboardCopyHandle(string textCopy, bool isDirectory)
    {
        await JsRuntimeRepo.InvokeVoidAsync("clipboardCopy.copyText", textCopy);
        SnackBarRepo.Info($"Путь к {(isDirectory ? "папке" : "файлу")} `{textCopy}` скопирован в буфер обмена");
    }
}