////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Retail.Wallet;
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

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public int PaymentId { get; set; }

    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }


    PaymentRetailDocumentModelDB? currentDoc, editDoc;
    UserInfoModel? userRecipient;
    WalletSelectInputComponent? recipientWalletRef;

    DateTime? DatePayment
    {
        get => editDoc?.DatePayment;
        set
        {
            if (editDoc is null)
                return;

            editDoc.DatePayment = value ?? DateTime.Now;
        }
    }

    bool CannotSave
    {
        get
        {
            if (currentDoc is null || editDoc?.Wallet is null || editDoc.Amount <= 0 || DatePayment is null || DatePayment == default)
                return true;

            return
                currentDoc.Id > 0 &&
                currentDoc.WalletId == editDoc.WalletId &&
                currentDoc.TypePayment == editDoc.TypePayment &&
                currentDoc.StatusPayment == editDoc.StatusPayment &&
                currentDoc.PaymentSource == editDoc.PaymentSource &&
                currentDoc.Description == editDoc.Description &&
                currentDoc.DatePayment == editDoc.DatePayment &&
                currentDoc.Amount == editDoc.Amount &&
                currentDoc.Name == editDoc.Name;
        }
    }


    async Task ResetEdit()
    {
        if (recipientWalletRef is null || editDoc is null)
            return;

        editDoc = GlobalTools.CreateDeepCopy(currentDoc)!;

        await recipientWalletRef.SetWallet(editDoc.Wallet);
    }

    async Task UpdateRecipient()
    {
        if (editDoc?.Wallet is null)
            throw new Exception("editDoc is null");

        await SetBusyAsync();
        TResponseModel<UserInfoModel[]> getUser = await IdentityRepo.GetUsersOfIdentityAsync([editDoc.Wallet.UserIdentityId]);
        SnackBarRepo.ShowMessagesResponse(getUser.Messages);
        if (getUser.Success() && getUser.Response is not null && getUser.Response.Any(x => x.UserId == editDoc.Wallet.UserIdentityId))
            userRecipient = getUser.Response.First(x => x.UserId == editDoc.Wallet.UserIdentityId);

        await SetBusyAsync(false);
    }

    async Task SaveDoc()
    {
        if (editDoc is null)
            throw new ArgumentNullException(nameof(editDoc));

        await SetBusyAsync();
        if (editDoc.Id <= 0)
        {
            TResponseModel<int> res = await RetailRepo.CreatePaymentDocumentAsync(editDoc);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success() && res.Response > 0)
                NavRepo.NavigateTo($"/retail/payment-document/{res.Response}");
        }
        else
        {
            ResponseBaseModel res = await RetailRepo.UpdatePaymentDocumentAsync(editDoc);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success())
            {
                TResponseModel<PaymentRetailDocumentModelDB[]>? getDoc = await RetailRepo.GetPaymentsDocumentsAsync(new() { Ids = [editDoc.Id] });
                SnackBarRepo.ShowMessagesResponse(getDoc.Messages);
                if (getDoc.Success() && getDoc.Response is not null && getDoc.Response.Length == 1)
                {
                    currentDoc = getDoc.Response[0];
                    editDoc = GlobalTools.CreateDeepCopy(currentDoc);
                }
            }
        }
        await SetBusyAsync(false);
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
                editDoc = GlobalTools.CreateDeepCopy(currentDoc);

                await UpdateRecipient();
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
        InvokeAsync(UpdateRecipient);
    }

    void SelectUserRecipientAction(UserInfoModel? user)
    {
        userRecipient = user;
    }
}