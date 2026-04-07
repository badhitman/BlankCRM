////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Telegram;

/// <summary>
/// MessagesTableTelegramComponent
/// </summary>
public partial class MessagesTableTelegramComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramBotStandardTransmission TelegramRepo { get; set; } = default!;


    /// <summary>
    /// ChatId
    /// </summary>
    [Parameter]
    public int ChatId { get; set; }


    MudTable<MessageTelegramStandardModel>? tableRef;

    async Task ReloadTableData()
    {
        if (tableRef is not null)
            await tableRef.ReloadServerData();
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    async Task<TableData<MessageTelegramStandardModel>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SearchMessagesChatStandardModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = DirectionsEnum.Down,
            Payload = new()
            {
                ChatId = ChatId
            }
        };
        TPaginationResponseStandardModel<MessageTelegramStandardModel> data = await TelegramRepo.MessagesTelegramSelectAsync(req, token);

        // Return the data
        return new TableData<MessageTelegramStandardModel>() { TotalItems = data.TotalRowsCount, Items = data.Response };
    }
}