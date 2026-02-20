////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using MudBlazor;

namespace BlazorLib.Components.Commerce.Organizations;

/// <summary>
/// BankDetailsEditComponent
/// </summary>
public partial class BankDetailsEditComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required IMudDialogInstance MudDialog { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required BankDetailsModelDB BankDetails { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action StateHasChangedHandler { get; set; }


    BankDetailsModelDB? bankDetailsEdit;

    async Task Submit()
    {
        if (bankDetailsEdit is null)
            throw new ArgumentNullException(nameof(bankDetailsEdit));
        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));
        if (BankDetails.Organization is null)
            throw new ArgumentNullException(nameof(BankDetails.Organization));

        await SetBusyAsync();
        TResponseModel<int> res = await CommerceRepo.BankDetailsUpdateOrCreateAsync(new TAuthRequestStandardModel<BankDetailsModelDB>()
        {
            Payload = bankDetailsEdit,
            SenderActionUserId = CurrentUserSession.UserId
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);

        if (res.Success())
        {
            BankDetails.Organization.BanksDetails ??= [];
            if (bankDetailsEdit.Id < 1)
            {
                bankDetailsEdit.Id = res.Response;
                BankDetails.Organization.BanksDetails.Add(bankDetailsEdit);
                if (BankDetails.Organization.BankMainAccount == 0 && BankDetails.Organization.BanksDetails?.Count == 1)
                    BankDetails.Organization.BankMainAccount = res.Response;
            }
            else
                BankDetails.Organization.BanksDetails.First(x => x.Id == bankDetailsEdit.Id).Update(bankDetailsEdit);
        }

        StateHasChangedHandler();
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        bankDetailsEdit = GlobalTools.CreateDeepCopy(BankDetails);
    }
}