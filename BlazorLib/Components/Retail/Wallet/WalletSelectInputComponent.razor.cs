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

    /// <inheritdoc/>
    [Parameter]
    public bool HideSystemWallets { get; set; }


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
        TResponseModel<UserInfoModel[]> getUser;
        if (!string.IsNullOrWhiteSpace(PresetClientId))
        {
            getUser = await IdentityRepo.GetUsersOfIdentityAsync([PresetClientId]);
            SnackBarRepo.ShowMessagesResponse(getUser.Messages);
            if (getUser.Response is not null && getUser.Response.Any(x => x.UserId == PresetClientId))
                currentUser = getUser.Response.First(x => x.UserId == PresetClientId);

            await ReloadWallets();
        }
        else if (WalletInit is not null)
        {
            getUser = await IdentityRepo.GetUsersOfIdentityAsync([WalletInit.UserIdentityId]);
            SnackBarRepo.ShowMessagesResponse(getUser.Messages);
            if (getUser.Response is not null && getUser.Response.Any(x => x.UserId == WalletInit.UserIdentityId))
                currentUser = getUser.Response.First(x => x.UserId == WalletInit.UserIdentityId);

            await ReloadWallets();
        }

        await SetBusyAsync(false);
    }

    async void SelectUserAction(UserInfoModel? selected)
    {
        if (currentUser?.UserId == selected?.UserId)
            return;

        await SetBusyAsync();
        currentUser = selected;
        currentWallet = null;

        if (selected is null || string.IsNullOrWhiteSpace(selected.UserId))
            walletsForSelect.Clear();
        else
            await ReloadWallets();

        await SetBusyAsync(false);

        if (SelectUserHandler is not null)
            SelectUserHandler(selected);
    }

    async Task ReloadWallets()
    {
        walletsForSelect.Clear();
        if (currentUser is null)
        {
            StateHasChanged();
            return;
        }
        await SetBusyAsync();
        TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> reqW = new()
        {
            PageSize = 100,
            Payload = new()
            {
                UsersFilterIdentityId = [currentUser.UserId],
                AutoGenerationWallets = true,
            }
        };
        TPaginationResponseModel<WalletRetailModelDB> getWallets = await RetailRepo.SelectWalletsAsync(reqW);
        SnackBarRepo.ShowMessagesResponse(getWallets.Status.Messages);

        if (getWallets.Response is null || getWallets.Response.Count == 0)
            SnackBarRepo.Error("Не удалось получить перечень кошельков пользователя");
        else
            walletsForSelect = [.. getWallets.Response.Where(x => !HideSystemWallets || x.WalletType?.IsSystem != true)];

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    public async Task SetWallet(WalletRetailModelDB? wallet)
    {
        if (currentWallet?.Id == wallet?.Id)
            return;

        currentWallet = wallet;

        if (wallet is not null)
        {
            TResponseModel<UserInfoModel[]> getUser = await IdentityRepo.GetUsersOfIdentityAsync([wallet.UserIdentityId]);
            SnackBarRepo.ShowMessagesResponse(getUser.Messages);
            if (getUser.Response is not null && getUser.Response.Any(x => x.UserId == PresetClientId))
                currentUser = getUser.Response.First(x => x.UserId == PresetClientId);
        }

        await ReloadWallets();
    }
}