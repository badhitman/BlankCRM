////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Retail.Wallet;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Conversions;

/// <summary>
/// ConversionDocumentComponent
/// </summary>
public partial class ConversionDocumentComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public int ConversionDocumentId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public DocumentRetailModelDB? InjectToOrder { get; set; }

    WalletConversionRetailDocumentModelDB? currentDoc, editDoc;
    UserInfoModel? userSender, userRecipient;
    WalletSelectInputComponent? senderWalletRef, recipientWalletRef;
    readonly List<ChatTelegramModelDB> currentChatTelegrams = [];

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

            if (DatePayment is null || DatePayment == default)
                return true;

            return
                currentDoc.Id > 0 &&
                currentDoc.ToWalletId == editDoc.ToWalletId &&
                currentDoc.FromWalletId == editDoc.FromWalletId &&
                currentDoc.ToWalletSum == editDoc.ToWalletSum &&
                currentDoc.DateDocument == editDoc.DateDocument &&
                currentDoc.FromWalletSum == editDoc.FromWalletSum &&
                currentDoc.Name == editDoc.Name;
        }
    }

    DateTime? datePayment;
    DateTime? DatePayment
    {
        get => datePayment;
        set
        {
            if (editDoc is null)
                return;

            datePayment = value ?? DateTime.Now;
            editDoc.DateDocument = datePayment ?? DateTime.Now;
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();

        if (ConversionDocumentId <= 0)
        {
            datePayment = DateTime.Now;
            currentDoc = new()
            {
                DateDocument = DateTime.Now,
            };
        }
        else
        {
            TResponseModel<WalletConversionRetailDocumentModelDB[]> getDocument = await RetailRepo.GetConversionsDocumentsAsync(new() { Ids = [ConversionDocumentId] });
            SnackBarRepo.ShowMessagesResponse(getDocument.Messages);
            if (getDocument.Success() && getDocument.Response is not null)
            {
                currentDoc = getDocument.Response.First();
                datePayment = currentDoc.DateDocument;
            }

            if (currentDoc?.FromWallet is null || currentDoc.ToWallet is null)
                SnackBarRepo.Error("currentDoc?.FromWallet is null || currentDoc.ToWallet is null");

        }
        editDoc = GlobalTools.CreateDeepCopy(currentDoc);

        await SetBusyAsync(false);
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
        currentChatTelegrams.Clear();
        List<long> _chatsIds = [];
        if (editDoc.FromWallet is not null)
        {
            userSender = UsersCache.First(x => x.UserId == editDoc.FromWallet.UserIdentityId);
            if (userSender.TelegramId.HasValue)
                _chatsIds.Add(userSender.TelegramId.Value);
        }

        if (editDoc.ToWallet is not null)
        {
            userRecipient = UsersCache.First(x => x.UserId == editDoc.ToWallet.UserIdentityId);
            if (userRecipient.TelegramId.HasValue)
                _chatsIds.Add(userRecipient.TelegramId.Value);
        }
        if (_chatsIds.Count != 0)
        {
            List<ChatTelegramModelDB> chats = await TelegramRepo.ChatsReadTelegramAsync([.. _chatsIds]);
            currentChatTelegrams.AddRange(chats);
        }
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
        if (editDoc is null)
            throw new ArgumentNullException(nameof(editDoc));

        await SetBusyAsync();
        if (editDoc.Id <= 0)
        {
            TResponseModel<int> res = await RetailRepo.CreateConversionDocumentAsync(CreateWalletConversionRetailDocumentRequestModel.Build(editDoc, InjectToOrder?.Id ?? 0));
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success() && res.Response > 0)
                NavRepo.NavigateTo($"/retail/conversion-document/{res.Response}");
        }
        else
        {
            ResponseBaseModel res = await RetailRepo.UpdateConversionDocumentAsync(editDoc);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success())
            {
                TResponseModel<WalletConversionRetailDocumentModelDB[]> getDoc = await RetailRepo.GetConversionsDocumentsAsync(new() { Ids = [editDoc.Id] });
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

    async Task ResetEdit()
    {
        if (senderWalletRef is null || recipientWalletRef is null || currentDoc is null || editDoc is null)
            return;

        editDoc = GlobalTools.CreateDeepCopy(currentDoc);

        await senderWalletRef.SetWallet(editDoc!.FromWallet);
        await recipientWalletRef.SetWallet(editDoc.ToWallet);
    }

    void OnFocusFromWalletSum()
    {
        if (editDoc is null)
            return;

        if (editDoc.FromWalletSum == 0 && editDoc.ToWalletSum > 0)
            editDoc.FromWalletSum = editDoc.ToWalletSum;
    }

    void OnFocusToWalletSum()
    {
        if (editDoc is null)
            return;

        if (editDoc.ToWalletSum == 0 && editDoc.FromWalletSum > 0)
            editDoc.ToWalletSum = editDoc.FromWalletSum;
    }
}