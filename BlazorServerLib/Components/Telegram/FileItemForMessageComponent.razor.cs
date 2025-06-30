﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Telegram;

/// <summary>
/// FileItemForMessageComponent
/// </summary>
public partial class FileItemForMessageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramTransmission telegramRepo { get; set; } = default!;

    [Inject]
    IJSRuntime JSRepo { get; set; } = default!;


    /// <summary>
    /// FileElement
    /// </summary>
    [Parameter, EditorRequired]
    public required FileBaseTelegramModel FileElement { get; set; }


    string? fileName;

    async Task DownloadFile()
    {
        await SetBusyAsync();
        TResponseModel<byte[]> rest = await telegramRepo.GetFileAsync(FileElement.FileId);
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (rest.Response is not null)
        {
            MemoryStream ms = new(rest.Response);
            using var streamRef = new DotNetStreamReference(stream: ms);
            await JSRepo.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
    }
}