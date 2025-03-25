////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorWebLib.Components.Commerce.Organizations;

/// <summary>
/// BanksListDetailsOrganizationComponent
/// </summary>
public partial class BanksListDetailsOrganizationComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IDialogService DialogService { get; set; } = default!;

    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required OrganizationModelDB CurrentOrganization { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public bool ReadOnly { get; set; }


    /// <inheritdoc/>
    public MudExpansionPanels? PanelsRef { get; private set; }

    /// <inheritdoc/>
    public bool IsExpanded { get; private set; }

    /// <inheritdoc/>
    void OnExpandedChanged(bool newVal)
    {
        if (ReadOnly)
        {
            IsExpanded = false;
            return;
        }

        IsExpanded = newVal;
    }

    async Task DeleteBankDetails(BankDetailsModelDB sender)
    {
        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        await SetBusy();
        bool? result = await DialogService.ShowMessageBox(
            "Внимание",
            "Подтверждаете удаление?",
            yesText: "Удалить!", cancelText: "Нет");

        if (result == true)
        {
            ResponseBaseModel res = await CommerceRepo.BankDetailsDeleteAsync(new TAuthRequestModel<int>() { Payload = sender.Id, SenderActionUserId = CurrentUserSession.UserId });
            SnackbarRepo.ShowMessagesResponse(res.Messages);

            CurrentOrganization.BanksDetails?.RemoveAll(x => x.Id == sender.Id);
            if (CurrentOrganization.BankMainAccount == sender.Id)
                CurrentOrganization.BankMainAccount = 0;
        }

        await SetBusy(false);
    }

    async Task SetBankDetailsAsMain(BankDetailsModelDB sender)
    {
        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        CurrentOrganization.BankMainAccount = sender.Id;
        await SetBusy();
        TResponseModel<int> res = await CommerceRepo.OrganizationUpdateAsync(new() { Payload = CurrentOrganization, SenderActionUserId = CurrentUserSession.UserId });
        await SetBusy(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
    }

    private Task<IDialogReference> CreateNewBankDetails()
    {
        DialogParameters<BankDetailsEditComponent> parameters = new()
        {
            { x => x.BankDetails, BankDetailsModelDB.BuildEmpty(CurrentOrganization) },
            { x => x.StateHasChangedHandler, StateHasChangedCall }
        };
        DialogOptions options = new() { CloseOnEscapeKey = true };

        return DialogService.ShowAsync<BankDetailsEditComponent>("Банковские реквизиты", parameters, options);
    }

    private Task<IDialogReference> OpenBankDetails(BankDetailsModelDB sender)
    {
        sender.Organization = CurrentOrganization;
        DialogParameters<BankDetailsEditComponent> parameters = new()
        {
            { x => x.BankDetails, sender },
            { x => x.StateHasChangedHandler, StateHasChangedCall }
        };
        DialogOptions options = new() { CloseOnEscapeKey = true };

        return DialogService.ShowAsync<BankDetailsEditComponent>("Банковские реквизиты", parameters, options);
    }
}