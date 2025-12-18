////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Wallet;

/// <summary>
/// WalletRetailTypeElementComponent
/// </summary>
public partial class WalletRetailTypeElementComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required WalletRetailTypeViewModel WalletTypeElement { get; set; }

    WalletRetailTypeViewModel? _walletCopy;

    bool IsEdited => _walletCopy is not null && !WalletTypeElement.Equals(_walletCopy);

    void InitEdit()
    {
        _walletCopy = GlobalTools.CreateDeepCopy(WalletTypeElement)!;
    }

    void CancelEdit()
    {
        _walletCopy = null;
    }

    async Task ChangeState(PaymentsRetailTypesEnum prt, WalletRetailTypeViewModel walletType)
    {
        await SetBusyAsync();
        ResponseBaseModel res = await RetailRepo.ToggleWalletTypeDisabledForPaymentTypeAsync(new() { PaymentType = prt, WalletTypeId = walletType.Id });
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        TResponseModel<WalletRetailTypeViewModel[]> getWT = await RetailRepo.WalletsTypesGetAsync([walletType.Id]);
        walletType.DisabledPaymentsTypes = getWT.Response?.FirstOrDefault()?.DisabledPaymentsTypes;

        await SetBusyAsync(false);
    }

    async Task Save()
    {
        if (_walletCopy is null)
        {
            SnackBarRepo.Error("_walletCopy is null");
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel resUpd = await RetailRepo.UpdateWalletTypeAsync(_walletCopy);
        if (!resUpd.Success())
        {
            await SetBusyAsync(false);
            SnackBarRepo.ShowMessagesResponse(resUpd.Messages);
            return;
        }

        TResponseModel<WalletRetailTypeViewModel[]>? resGet = await RetailRepo.WalletsTypesGetAsync([_walletCopy.Id]);
        if (!resGet.Success() || resGet.Response is null || resGet.Response.Length != 1)
        {
            await SetBusyAsync(false);
            SnackBarRepo.ShowMessagesResponse(resUpd.Messages);
            return;
        }

        WalletTypeElement.Name = resGet.Response[0].Name;
        WalletTypeElement.Description = resGet.Response[0].Description;
        WalletTypeElement.IsDisabled = resGet.Response[0].IsDisabled;
        WalletTypeElement.IsSystem = resGet.Response[0].IsSystem;
        WalletTypeElement.IgnoreBalanceChanges = resGet.Response[0].IgnoreBalanceChanges;
        WalletTypeElement.LastUpdatedAtUTC = resGet.Response[0].LastUpdatedAtUTC;

        _walletCopy = null;

        await SetBusyAsync(false);
    }
}