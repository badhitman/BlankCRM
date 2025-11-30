////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Wallet;

/// <summary>
/// WalletRetailTypesComponent
/// </summary>
public partial class WalletRetailTypesComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService retailRepo { get; set; } = default!;


    List<WalletRetailTypeViewModel>? WalletsTypesList;

    WalletRetailTypeViewModel creatingNewWallet = new();

    async Task CreateNew()
    {
        await SetBusyAsync();
        TResponseModel<int>? resCreate = await retailRepo.CreateWalletTypeAsync(creatingNewWallet);
        if(!resCreate.Success())
        {
            SnackBarRepo.ShowMessagesResponse(resCreate.Messages);
            await SetBusyAsync(false);
            return;
        }

        creatingNewWallet = new();
        TPaginationResponseModel<WalletRetailTypeViewModel>? res = await retailRepo.SelectWalletsTypesAsync(new TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel>() { PageSize = int.MaxValue });
        WalletsTypesList = res.Response;
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        TPaginationResponseModel<WalletRetailTypeViewModel>? res = await retailRepo.SelectWalletsTypesAsync(new TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel>() { PageSize = int.MaxValue });
        WalletsTypesList = res.Response;
        await SetBusyAsync(false);
    }
}