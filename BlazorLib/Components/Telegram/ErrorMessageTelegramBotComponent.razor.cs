////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Telegram;

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


    private IEnumerable<ErrorSendingMessageTelegramBotStandardModel> pagedData = default!;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<ErrorSendingMessageTelegramBotStandardModel>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotStandardModel> err_res = await TelegramRepo.ErrorsForChatsSelectTelegramAsync(new TPaginationRequestStandardModel<long[]>()
        {
            Payload = ChatId is null || ChatId.Value == 0 ? null : [ChatId.Value],
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        }, token);

        await SetBusyAsync(false, token);

        if (err_res.Response is null)
            return new TableData<ErrorSendingMessageTelegramBotStandardModel>() { TotalItems = 0, Items = [] };

        pagedData = err_res.Response;
        return new TableData<ErrorSendingMessageTelegramBotStandardModel>() { TotalItems = err_res.TotalRowsCount, Items = pagedData };
    }
}