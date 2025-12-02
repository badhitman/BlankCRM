////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Clients;

/// <summary>
/// ClientAboutComponent
/// </summary>
public partial class ClientAboutComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;

    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string ClientId { get; set; }


    UserInfoModel? currentUser;
    ChatTelegramModelDB? currentChatTelegram;
    List<WalletRetailModelDB>? WalletsForUser;

    /// <inheritdoc/>
    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        TResponseModel<UserInfoModel[]> getUser = await IdentityRepo.GetUsersOfIdentityAsync([ClientId]);
        SnackBarRepo.ShowMessagesResponse(getUser.Messages);
        currentUser = getUser.Response?.FirstOrDefault();

        if(currentUser is not null && currentUser.TelegramId.HasValue)
        {
            List<ChatTelegramModelDB> chats = await TelegramRepo.ChatsReadTelegramAsync([currentUser.TelegramId.Value]);
            currentChatTelegram = chats.FirstOrDefault();
        }

        TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> reqWallets = new()
        {
            PageNum = 0,
            PageSize = int.MaxValue,
            Payload = new()
            {
                UsersFilterIdentityId = [ClientId],
                AutoGenerationWallets = true,
            }
        };
        TPaginationResponseModel<WalletRetailModelDB>? resWallets = await RetailRepo.SelectWalletsAsync(reqWallets);
        WalletsForUser = resWallets.Response;

        await SetBusyAsync(false);
    }
}