////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Wallet;

/// <summary>
/// WalletSelectInputComponent
/// </summary>
public partial class WalletSelectInputComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <summary>
    /// Предустановленный пользователь
    /// </summary>
    /// <remarks>
    /// Если указан, то сменить его нельзя
    /// </remarks>
    [Parameter]
    public string? PresetClientId { get; set; }

    /// <summary>
    /// Выбранный кошелёк
    /// </summary>
    [Parameter]
    public WalletRetailModelDB? WalletInit { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public string? Title { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public Action<WalletRetailModelDB?> SelectWalletHandler { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public Action<UserInfoModel?>? SelectUserHandler { get; set; }


    UserInfoModel? currentUser;

    WalletRetailModelDB? currentWallet;
    WalletRetailModelDB? CurrentWallet
    {
        get => currentWallet;
        set
        {
            currentWallet = value;
            SelectWalletHandler(value);
        }
    }

    List<WalletRetailModelDB> walletsForSelect = [];

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        currentWallet = WalletInit;
        await SetBusyAsync();
        if (!string.IsNullOrWhiteSpace(PresetClientId))
        {
            TResponseModel<UserInfoModel[]> getUser = await IdentityRepo.GetUsersOfIdentityAsync([PresetClientId]);
            SnackBarRepo.ShowMessagesResponse(getUser.Messages);
            if (getUser.Response is not null && getUser.Response.Any(x => x.UserId == PresetClientId))
                currentUser = getUser.Response.First(x => x.UserId == PresetClientId);

            await ReloadWallets(PresetClientId);
        }
        else if (WalletInit is not null)
            await ReloadWallets(WalletInit.UserIdentityId);

        await SetBusyAsync(false);
    }

    async void SelectUserAction(UserInfoModel? selected)
    {
        await SetBusyAsync();
        currentUser = selected;
        CurrentWallet = null;

        if (selected is null || string.IsNullOrWhiteSpace(selected.UserId))
            walletsForSelect.Clear();
        else
            await ReloadWallets(selected.UserId);

        await SetBusyAsync(false);

        if (SelectUserHandler is not null)
            SelectUserHandler(selected);
    }

    async Task ReloadWallets(string userId)
    {
        TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> reqW = new()
        {
            Payload = new()
            {
                UsersFilterIdentityId = [userId],
                AutoGenerationWallets = true,
            }
        };
        TPaginationResponseModel<WalletRetailModelDB> getWallets = await RetailRepo.SelectWalletsAsync(reqW);
        SnackBarRepo.ShowMessagesResponse(getWallets.Status.Messages);
        if (getWallets.Response is null || getWallets.Response.Count == 0)
            walletsForSelect.Clear();
        else
        {
            walletsForSelect = getWallets.Response;
        }
    }

    internal async Task SetWallet(WalletRetailModelDB? fromWallet)
    {
        throw new NotImplementedException();
    }
}