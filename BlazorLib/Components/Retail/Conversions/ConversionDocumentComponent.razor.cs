////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Conversions;

public partial class ConversionDocumentComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public string? ClientId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public int ConversionDocumentId { get; set; }


    WalletConversionRetailDocumentModelDB? currentDoc, editDoc;
    UserInfoModel? userSender, userRecipient;

    bool CannotSave
    {
        get
        {
            if (currentDoc is null || editDoc?.FromWallet is null || editDoc.ToWallet is null)
                return true;

            if (editDoc.FromWallet.Id == editDoc.ToWallet.Id)
                return true;

            if (editDoc.FromWalletSum <= 0 || editDoc.ToWalletSum <= 0)
                return true;

            return
                currentDoc.Id > 0 &&
                currentDoc.ToWalletId == editDoc.ToWalletId &&
                currentDoc.FromWalletId == editDoc.FromWalletId &&
                currentDoc.ToWalletSum == editDoc.ToWalletSum &&
                currentDoc.FromWalletSum == editDoc.FromWalletSum &&
                currentDoc.Name == editDoc.Name &&
                currentDoc.Description == editDoc.Description;
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (ConversionDocumentId <= 0)
            currentDoc = new() { };
        else
        {
            await SetBusyAsync();
            TResponseModel<WalletConversionRetailDocumentModelDB[]> getDocument = await RetailRepo.GetConversionsDocumentsAsync(new() { Ids = [ConversionDocumentId] });
            SnackBarRepo.ShowMessagesResponse(getDocument.Messages);
            if (getDocument.Success() && getDocument.Response is not null)
                currentDoc = getDocument.Response.First();


            await SetBusyAsync(false);
        }

        editDoc = GlobalTools.CreateDeepCopy(currentDoc);
        await UpdateUsers();
    }

    async Task UpdateUsers()
    {
        if (editDoc is null)
            return;

        List<string> usersIds = [];

        if (editDoc.FromWallet is not null)
            usersIds.Add(editDoc.FromWallet.UserIdentityId);
        if (editDoc.ToWallet is not null)
            usersIds.Add(editDoc.ToWallet.UserIdentityId);

        if (usersIds.Count == 0)
            return;

        await CacheUsersUpdate([.. usersIds]);

        if (editDoc.FromWallet is not null)
            userSender = UsersCache.First(x => x.UserId == editDoc.FromWallet.UserIdentityId);

        if (editDoc.ToWallet is not null)
            userRecipient = UsersCache.First(x => x.UserId == editDoc.ToWallet.UserIdentityId);
    }

    void SelectUserSenderAction(UserInfoModel? user)
    {
        userSender = user;
        StateHasChanged();
    }

    void SelectWalletSenderAction(WalletRetailModelDB? wallet)
    {
        if (editDoc is null)
            throw new ArgumentNullException(nameof(editDoc));

        editDoc.FromWallet = wallet;
        editDoc.FromWalletId = wallet?.Id ?? 0;
        InvokeAsync(UpdateUsers);
        StateHasChanged();
    }

    void SelectUserRecipientAction(UserInfoModel? user)
    {
        userRecipient = user;
        StateHasChanged();
    }

    void SelectWalletRecipientAction(WalletRetailModelDB? wallet)
    {
        if (editDoc is null)
            throw new ArgumentNullException(nameof(editDoc));

        editDoc.ToWallet = wallet;
        editDoc.ToWalletId = wallet?.Id ?? 0;
        InvokeAsync(UpdateUsers);
        StateHasChanged();
    }

    async Task SaveDoc()
    {
        await SetBusyAsync();

        await SetBusyAsync(false);
    }

    void ResetEdit()
        => editDoc = GlobalTools.CreateDeepCopy(currentDoc);
}