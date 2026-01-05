////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Telegram;

/// <summary>
/// ChatsTableTelegramComponent
/// </summary>
public partial class ChatsTableTelegramComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramBotStandardTransmission TelegramRepo { get; set; } = default!;


    MudTable<ChatTelegramViewModel>? tableRef;

    async Task ReloadTableData()
    {
        if (tableRef is not null)
            await tableRef.ReloadServerData();
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    async Task<TableData<ChatTelegramViewModel>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<string?> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = DirectionsEnum.Down,
        };
        TPaginationResponseStandardModel<ChatTelegramViewModel> data = await TelegramRepo.ChatsSelectTelegramAsync(req, token);

        // Return the data
        return new TableData<ChatTelegramViewModel>() { TotalItems = data.TotalRowsCount, Items = data.Response };
    }
}