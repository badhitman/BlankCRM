////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Payments;

/// <summary>
/// PaymentDocumentComponent
/// </summary>
public partial class PaymentDocumentComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public int PaymentId { get; set; }

    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }


    PaymentRetailDocumentModelDB? currentDoc, editDoc;
    WalletRetailModelDB? currentWallet;
    UserInfoModel? userRecipient;

    DateTime? datePayment;
    DateTime? DatePayment
    {
        get => datePayment;
        set
        {
            if (editDoc is null)
                return;

            datePayment = value ?? DateTime.Now;
            editDoc.DatePayment = datePayment ?? DateTime.Now;
        }
    }


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();

        if (CurrentUserSession is null)
            throw new InvalidOperationException("CurrentUserSession is null");

        if (PaymentId < 1)
        {
            currentDoc = new()
            {
                DatePayment = DateTime.UtcNow,
                StatusPayment = PaymentsRetailStatusesEnum.Awaiting,
                AuthorUserIdentity = CurrentUserSession.UserId,
            };
            editDoc = GlobalTools.CreateDeepCopy(currentDoc);
        }
        else
        {
            TResponseModel<PaymentRetailDocumentModelDB[]> resDoc = await RetailRepo.GetPaymentsDocumentsAsync(new() { Ids = [PaymentId] });
            SnackBarRepo.ShowMessagesResponse(resDoc.Messages);
            if (resDoc.Response is not null && resDoc.Response.Length == 1)
            {
                currentDoc = resDoc.Response[0];
                editDoc = GlobalTools.CreateDeepCopy(editDoc);

                currentWallet = editDoc!.Wallet;
                TResponseModel<UserInfoModel[]> getUser = await IdentityRepo.GetUsersOfIdentityAsync([currentWallet!.UserIdentityId]);
                SnackBarRepo.ShowMessagesResponse(getUser.Messages);
                if (getUser.Success() && getUser.Response is not null && getUser.Response.Any(x => x.UserId == editDoc.Wallet!.UserIdentityId))
                    userRecipient = getUser.Response.First(x => x.UserId == currentWallet.UserIdentityId);
            }
        }
        await SetBusyAsync(false);
    }

    void SelectWalletRecipientAction(WalletRetailModelDB? wallet)
    {
        if (editDoc is null)
            throw new ArgumentNullException(nameof(editDoc));

        editDoc.Wallet = wallet;
        editDoc.WalletId = wallet?.Id ?? 0;
        //InvokeAsync(UpdateUsers);
        StateHasChanged();
    }

    void SelectUserRecipientAction(UserInfoModel? user)
    {
        //userRecipient = user;
        StateHasChanged();
    }
}