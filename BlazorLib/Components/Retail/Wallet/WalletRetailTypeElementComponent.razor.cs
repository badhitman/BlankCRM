////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorLib.Components.Retail.Wallet;

/// <summary>
/// WalletRetailTypeElementComponent
/// </summary>
public partial class WalletRetailTypeElementComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission RubricsRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required WalletRetailTypeViewModel WalletTypeElement { get; set; }


    bool CannotSave => !IsEdited || IsBusyProgress;

    WalletRetailTypeViewModel? _walletCopy;
    List<UniversalBaseModel> AllPaymentsTypes = [];
    bool IsEdited => _walletCopy is not null && !WalletTypeElement.Equals(_walletCopy);


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        string ctx = Path.Combine(Routes.PAYMENTS_CONTROLLER_NAME, Routes.TYPES_CONTROLLER_NAME);
        AllPaymentsTypes = await RubricsRepo.RubricsChildListAsync(new() { ContextName = ctx });
        await SetBusyAsync(false);
    }

    void InitEdit()
    {
        _walletCopy = GlobalTools.CreateDeepCopy(WalletTypeElement)!;
    }

    void CancelEdit()
    {
        _walletCopy = null;
    }

    async Task ChangeState(int prt, WalletRetailTypeViewModel walletType)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel res = await RetailRepo.ToggleWalletTypeDisabledForPaymentTypeAsync(new()
        {
            Payload = new()
            {
                PaymentType = prt,
                WalletTypeId = walletType.Id
            },
            SenderActionUserId = CurrentUserSession.UserId,
        });
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
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("_walletCopy is null");
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel resUpd = await RetailRepo.UpdateWalletTypeAsync(new() { Payload = _walletCopy, SenderActionUserId = CurrentUserSession.UserId });
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