﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Telegram;

/// <summary>
/// ErrorMessageTelegramBotComponent
/// </summary>
public partial class ErrorMessageTelegramBotComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;


    /// <summary>
    /// ChatId
    /// </summary>
    [Parameter]
    public long? ChatId { get; set; }


    private IEnumerable<ErrorSendingMessageTelegramBotModelDB> pagedData = default!;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<ErrorSendingMessageTelegramBotModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB> err_res = await TelegramRepo.ErrorsForChatsSelectTelegramAsync(new TPaginationRequestStandardModel<long[]?>()
        {
            Payload = ChatId is null || ChatId.Value == 0 ? null : [ChatId.Value],
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
        }, token);
        IsBusyProgress = false;
        
        if (err_res.Response is null)
            return new TableData<ErrorSendingMessageTelegramBotModelDB>() { TotalItems = 0, Items = [] };

        pagedData = err_res.Response;
        return new TableData<ErrorSendingMessageTelegramBotModelDB>() { TotalItems = err_res.TotalRowsCount, Items = pagedData };
    }
}