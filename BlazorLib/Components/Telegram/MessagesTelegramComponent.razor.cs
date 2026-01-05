////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Telegram;

/// <summary>
/// MessagesTelegramComponent
/// </summary>
public partial class MessagesTelegramComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;


    /// <summary>
    /// Id - of database
    /// </summary>
    [Parameter, EditorRequired]
    public int ChatId { get; set; }


    ChatTelegramModelDB? CurrentChat;

    private string _searchStringQuery = "";
    private string SearchStringQuery
    {
        get => _searchStringQuery;
        set
        {
            _searchStringQuery = value;
            if (TableRef is not null)
                InvokeAsync(TableRef.ReloadServerData);
        }
    }

    /// <summary>
    /// Table
    /// </summary>
    public MudTable<MessageTelegramModelDB>? TableRef { get; set; }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        CurrentChat = await TelegramRepo.ChatTelegramReadAsync(ChatId);
        await SetBusyAsync(false);
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<MessageTelegramModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<MessageTelegramModelDB> rest_message = await TelegramRepo
            .MessagesTelegramSelectAsync(new TPaginationRequestStandardModel<SearchMessagesChatModel>()
            {
                Payload = new() { ChatId = ChatId, SearchQuery = SearchStringQuery },
                PageNum = state.Page,
                PageSize = state.PageSize,
                SortingDirection = state.SortDirection.Convert(),
                SortBy = state.SortLabel,
            }, token);

        await SetBusyAsync(false, token);

        if (rest_message.Response is null)
            return new TableData<MessageTelegramModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<MessageTelegramModelDB>() { TotalItems = rest_message.TotalRowsCount, Items = rest_message.Response };
    }
}