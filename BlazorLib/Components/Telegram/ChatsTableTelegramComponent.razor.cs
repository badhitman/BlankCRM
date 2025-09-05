﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Telegram;

public partial class ChatsTableTelegramComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramBotStandardTransmission TelegramRepo { get; set; } = default!;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    async Task<TableData<ChatTelegramViewModel>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<string?> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,

        };
        TPaginationResponseModel<ChatTelegramViewModel> data = await TelegramRepo.ChatsSelectTelegramAsync(req, token);

        // Return the data
        return new TableData<ChatTelegramViewModel>() { TotalItems = data.TotalRowsCount, Items = data.Response };
    }
}