////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Delivery;

public partial class DeliveryDocumentComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public int DeliveryDocumentId { get; set; }


    DeliveryDocumentRetailModelDB? currentDoc, editDoc;
    UserInfoModel? senderUser;

    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;


    bool CannotSave
    {
        get
        {
            if (currentDoc is null || editDoc is null || string.IsNullOrWhiteSpace(editDoc.KladrCode))
                return true;

            if (string.IsNullOrWhiteSpace(editDoc.RecipientIdentityUserId) || editDoc.ShippingCost <= 0 || editDoc.WeightShipping <= 0)
                return true;

            return
                currentDoc.Id > 0 &&
                currentDoc.Description == editDoc.Description &&
                currentDoc.Name == editDoc.Name;
        }
    }

    async void SelectUserHandler(UserInfoModel? selected)
    {
        if (editDoc is null || selected?.UserId == senderUser?.UserId)
            return;

        editDoc.RecipientIdentityUserId = selected?.UserId ?? "";
        if (string.IsNullOrWhiteSpace(editDoc.RecipientIdentityUserId))
            senderUser = null;
        else
        {
            TResponseModel<UserInfoModel[]> getUsers = await IdentityRepo.GetUsersOfIdentityAsync([editDoc.RecipientIdentityUserId]);
            SnackBarRepo.ShowMessagesResponse(getUsers.Messages);
            if (getUsers.Success() && getUsers.Response is not null && getUsers.Response.Any(x => x.UserId == editDoc.RecipientIdentityUserId))
            {
                senderUser = getUsers.Response.First(x => x.UserId == editDoc.RecipientIdentityUserId);
            }
        }
    }

    EntryAltModel? SelectedKladrObject
    {
        get => EntryAltModel.Build(editDoc?.KladrCode ?? "", editDoc?.KladrTitle ?? "");
        set
        {
            if (editDoc is null)
                return;

            editDoc.KladrCode = value?.Id ?? "";
            editDoc.KladrTitle = value?.Name ?? "";
        }
    }


    void AddressUserCommentHandleOnChange(ChangeEventArgs args)
    {
        if (editDoc is null || editDoc is null)
        {
            SnackBarRepo.Error("editDoc is null || editDoc is null");
            return;
        }

        editDoc.AddressUserComment = args.Value?.ToString();
    }

    void ResetEdit()
    {
        editDoc = GlobalTools.CreateDeepCopy(currentDoc)!;
    }

    async Task SaveDoc()
    {
        if (editDoc is null)
            throw new ArgumentNullException(nameof(editDoc));

        await SetBusyAsync();
        if (editDoc.Id <= 0)
        {
            TResponseModel<int> res = await RetailRepo.CreateDeliveryDocumentAsync(editDoc);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success() && res.Response > 0)
                NavRepo.NavigateTo($"/retail/delivery-document/{res.Response}");
        }
        else
        {
            ResponseBaseModel res = await RetailRepo.UpdateDeliveryDocumentAsync(editDoc);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success())
            {
                TResponseModel<DeliveryDocumentRetailModelDB[]> getDoc = await RetailRepo.GetDeliveryDocumentsAsync(new() { Ids = [editDoc.Id] });
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
        await base.OnInitializedAsync();
        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{Routes.DELIVERY_CONTROLLER_NAME}/{Routes.DOCUMENT_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={Routes.IMAGE_ACTION_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={DeliveryDocumentId}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);


        if (DeliveryDocumentId > 0)
        {
            await SetBusyAsync();
            TResponseModel<DeliveryDocumentRetailModelDB[]>? res = await RetailRepo.GetDeliveryDocumentsAsync(new() { Ids = [DeliveryDocumentId] });
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            if (res.Response is not null && res.Response.Length == 1)
            {
                currentDoc = res.Response.First();

            }

            await SetBusyAsync(false);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(ClientId))
            {

                TResponseModel<UserInfoModel[]> getUsers = await IdentityRepo.GetUsersOfIdentityAsync([ClientId]);
                SnackBarRepo.ShowMessagesResponse(getUsers.Messages);
                if (getUsers.Success() && getUsers.Response is not null && getUsers.Response.Any(x => x.UserId == ClientId))
                {
                    senderUser = getUsers.Response.First(x => x.UserId == ClientId);
                }
            }

            currentDoc = new()
            {
                AuthorIdentityUserId = CurrentUserSession.UserId,
                RecipientIdentityUserId = ClientId ?? CurrentUserSession.UserId,
            };
        }
        editDoc = GlobalTools.CreateDeepCopy(currentDoc);
    }
}