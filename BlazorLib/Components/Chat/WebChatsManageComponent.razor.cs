////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Chat;

/// <summary>
/// WebChatsManageComponent
/// </summary>
public partial class WebChatsManageComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IWebChatService WebChatRepo { get; set; } = default!;

    MudTable<DialogWebChatModelDB>? tableRef;

    async Task JoinToChat(int chatId, bool isExclusive = false)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        TAuthRequestStandardModel<UserInjectDialogWebChatRequestModel> req = new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                UserIdentityId = CurrentUserSession.UserId,
                DialogJoinId = chatId,
                IsExclusiveJoin = isExclusive,
            }
        };
        await SetBusyAsync();
        ResponseBaseModel res = await WebChatRepo.UserInjectDialogWebChatAsync(req);
        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    async Task<TableData<DialogWebChatModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectDialogsWebChatsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            Payload = new()
            {

            }
        };
        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<DialogWebChatModelDB> res = await WebChatRepo.SelectDialogsWebChatsAsync(req, token);

        if (res.Response is not null && res.Response.Count != 0)
            await CacheUsersUpdate([.. res.Response.Where(x => !string.IsNullOrWhiteSpace(x.InitiatorIdentityId)).Select(x => x.InitiatorIdentityId)!]);

        await SetBusyAsync(false, token);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);
        return new TableData<DialogWebChatModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}