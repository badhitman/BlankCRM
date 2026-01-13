////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using MudBlazor;

namespace BlazorLib.Components.Commerce.Organizations;

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

        await SetBusyAsync();
        bool? result = await DialogService.ShowMessageBox(
            "Внимание",
            "Подтверждаете удаление?",
            yesText: "Удалить!", cancelText: "Нет");

        if (result == true)
        {
            ResponseBaseModel res = await CommerceRepo.BankDetailsForOrganizationDeleteAsync(new TAuthRequestStandardModel<int>() { Payload = sender.Id, SenderActionUserId = CurrentUserSession.UserId });
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            CurrentOrganization.BanksDetails?.RemoveAll(x => x.Id == sender.Id);
            if (CurrentOrganization.BankMainAccount == sender.Id)
                CurrentOrganization.BankMainAccount = 0;
        }

        await SetBusyAsync(false);
    }

    async Task SetBankDetailsAsMain(BankDetailsModelDB sender)
    {
        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        CurrentOrganization.BankMainAccount = sender.Id;
        await SetBusyAsync();
        TResponseModel<int> res = await CommerceRepo.OrganizationUpdateOrCreateAsync(new() { Payload = CurrentOrganization, SenderActionUserId = CurrentUserSession.UserId });
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
    }

    private Task<IDialogReference> CreateNewBankDetails()
    {
        DialogParameters<BankDetailsEditComponent> parameters = new()
        {
            { x => x.BankDetails, BankDetailsModelDB.BuildEmpty(CurrentOrganization) },
            { x => x.StateHasChangedHandler, StateHasChangedCall }
        };
        DialogOptions options = new()
        {
            CloseButton = true,
            CloseOnEscapeKey = true,
        };

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