////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Telegram;

/// <summary>
/// FileItemForMessageComponent
/// </summary>
public partial class FileItemForMessageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;

    [Inject]
    IJSRuntime JSRepo { get; set; } = default!;


    /// <summary>
    /// FileElement
    /// </summary>
    [Parameter, EditorRequired]
    public required FileBaseTelegramStandardModel FileElement { get; set; }


    string? fileName;

    async Task DownloadFile()
    {
        if(string.IsNullOrWhiteSpace(FileElement.FileId))
        {
            SnackBarRepo.Error("string.IsNullOrWhiteSpace(FileElement.FileId)");
            return; 
        }

        await SetBusyAsync();
        TResponseModel<byte[]> rest = await TelegramRepo.GetFileTelegramAsync(FileElement.FileId);

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (rest.Response is not null)
        {
            MemoryStream ms = new(rest.Response);
            using DotNetStreamReference? streamRef = new DotNetStreamReference(stream: ms);
            await JSRepo.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }

        await SetBusyAsync(false);
    }
}