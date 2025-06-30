﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Telegram;

/// <summary>
/// TelegramChatWrapComponent
/// </summary>
public partial class TelegramChatWrapComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;


    /// <summary>
    /// Чат
    /// </summary>
    [Parameter, EditorRequired]
    public required ChatTelegramModelDB Chat { get; set; }

    /// <summary>
    /// SendMessageHandle
    /// </summary>
    [CascadingParameter]
    public Action<SendTextMessageTelegramBotModel>? SendMessageHandle { get; set; }


    private string _inputFileId = Guid.NewGuid().ToString();

    string? _textSendMessage;

    MessagesTelegramComponent _messagesTelegramComponent = default!;

    bool NavbarToggle = true;

    private readonly List<IBrowserFile> loadedFiles = [];

    async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage))
            throw new ArgumentNullException(nameof(_textSendMessage));

        await SetBusyAsync();
        SendTextMessageTelegramBotModel req = new() { Message = _textSendMessage, UserTelegramId = Chat.ChatTelegramId, From = "Техподдержка" };

        MemoryStream ms;
        if (loadedFiles.Count != 0)
        {
            req.Files = [];

            foreach (var fileBrowser in loadedFiles)
            {
                ms = new();
                await fileBrowser.OpenReadStream(maxAllowedSize: 1024 * 18 * 1024).CopyToAsync(ms);
                req.Files.Add(new() { ContentType = fileBrowser.ContentType, Name = fileBrowser.Name, Data = ms.ToArray() });
                await ms.DisposeAsync();
            }
        }

        TResponseModel<MessageComplexIdsModel> rest = await TelegramRepo.SendTextMessageTelegramAsync(req);
        _textSendMessage = "";
        if (_messagesTelegramComponent.TableRef is not null)
            await _messagesTelegramComponent.TableRef.ReloadServerData();

        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        loadedFiles.Clear();
        _inputFileId = Guid.NewGuid().ToString();
        if (SendMessageHandle is not null)
            SendMessageHandle(req);
    }

    void SelectFilesChange(InputFileChangeEventArgs e)
    {
        loadedFiles.Clear();

        foreach (IBrowserFile file in e.GetMultipleFiles())
        {
            loadedFiles.Add(file);
        }
    }

    string GetTitle()
    {
        if (!string.IsNullOrWhiteSpace(Chat.Username))
            return $"@{Chat.Username}";

        if (!string.IsNullOrWhiteSpace(Chat.Title))
            return $"{Chat.Title} /{Chat.FirstName} {Chat.LastName}";

        if (!string.IsNullOrWhiteSpace(Chat.FirstName) || !string.IsNullOrWhiteSpace(Chat.LastName))
            return $"{Chat.FirstName} {Chat.LastName}";

        return $"#{Chat.ChatTelegramId}";
    }
}