////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Wallet;

/// <summary>
/// WalletRetailTypesComponent
/// </summary>
public partial class WalletRetailTypesComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    List<WalletRetailTypeViewModel>? WalletsTypesList;

    WalletRetailTypeViewModel creatingNewWallet = new();

    bool CannotCreateNew => string.IsNullOrWhiteSpace(creatingNewWallet.Name);


    async Task CreateNew()
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        await SetBusyAsync();
        TResponseModel<int>? resCreate = await RetailRepo.CreateWalletTypeAsync(new()
        {
            Payload = creatingNewWallet,
            SenderActionUserId = CurrentUserSession.UserId,
        });

        if (!resCreate.Success())
        {
            SnackBarRepo.ShowMessagesResponse(resCreate.Messages);
            await SetBusyAsync(false);
            return;
        }

        creatingNewWallet = new();
        TPaginationResponseStandardModel<WalletRetailTypeViewModel>? res = await RetailRepo.SelectWalletsTypesAsync(new TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel>() { PageSize = int.MaxValue });
        WalletsTypesList = res.Response;
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        TPaginationResponseStandardModel<WalletRetailTypeViewModel>? res = await RetailRepo.SelectWalletsTypesAsync(new TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel>() { PageSize = int.MaxValue });
        WalletsTypesList = res.Response;
        await SetBusyAsync(false);
    }
}