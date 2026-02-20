////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveryDocumentComponent
/// </summary>
public partial class DeliveryDocumentComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IRubricsTransmission RubricsRepo { get; set; } = default!;

    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;

    [Inject]
    IParametersStorageTransmission StorageTransmissionRepo { get; set; } = default!;

    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public int DeliveryDocumentId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public DocumentRetailModelDB? InjectToOrder { get; set; }

    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }


    List<RubricNestedModel> AllDeliveriesTypes = [];
    ChatTelegramStandardModel? currentChatTelegram;
    DeliveryDocumentRetailModelDB? currentDoc, editDoc;
    UserInfoModel? recipientUser;
    DeliveryTableRowsRetailComponent? tableRowsRef;
    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;
    decimal totalWeightOrdersDocumentsLinks;
    int rubricDeliveryTypePickup;

    bool CannotSave
    {
        get
        {
            if (IsBusyProgress || currentDoc is null || editDoc is null)
                return true;

            if (editDoc.DeliveryTypeId != rubricDeliveryTypePickup && string.IsNullOrWhiteSpace(editDoc.KladrCode) && string.IsNullOrWhiteSpace(editDoc.AddressUserComment))
                return true;

            if (string.IsNullOrWhiteSpace(editDoc.RecipientIdentityUserId) || editDoc.WarehouseId <= 0)
                return true;

            return
                currentDoc.Id > 0 &&
                currentDoc.Notes == editDoc.Notes &&
                currentDoc.PackageSize == editDoc.PackageSize &&
                currentDoc.DeliveryTypeId == editDoc.DeliveryTypeId &&
                currentDoc.RecipientIdentityUserId == editDoc.RecipientIdentityUserId &&
                currentDoc.ShippingCost == editDoc.ShippingCost &&
                currentDoc.WeightShipping == editDoc.WeightShipping &&
                currentDoc.Description == editDoc.Description &&
                currentDoc.WarehouseId == editDoc.WarehouseId &&
                currentDoc.AddressUserComment == editDoc.AddressUserComment &&
                currentDoc.KladrCode == editDoc.KladrCode &&
                currentDoc.Name == editDoc.Name;
        }
    }

    bool CanCopyAddressFromRecipient
    {
        get
        {
            if (editDoc is null || DeliveryAddressSelectedKladrObject is null || recipientUser is null)
                return false;

            if (string.IsNullOrWhiteSpace(recipientUser.KladrCode) || string.IsNullOrWhiteSpace(recipientUser.KladrTitle))
                return false;

            return recipientUser.KladrCode != editDoc.KladrCode;
        }
    }

    EntryAltStandardModel? DeliveryAddressSelectedKladrObject
    {
        get => EntryAltStandardModel.Build(editDoc?.KladrCode ?? "", editDoc?.KladrTitle ?? "");
        set
        {
            if (editDoc is null)
                return;

            editDoc.KladrCode = value?.Id ?? "";
            editDoc.KladrTitle = value?.Name ?? "";
        }
    }


    async void WarehouseSelectAction(RubricNestedModel? selectedWarehouse)
    {
        if (editDoc is null)
            throw new ArgumentNullException(nameof(editDoc));

        editDoc.WarehouseId = selectedWarehouse?.Id ?? 0;

        StateHasChanged();
        if (tableRowsRef is not null)
        {
            await tableRowsRef.LoadOffers(0);
            if (tableRowsRef.AddingDomRef is not null)
                await tableRowsRef.AddingDomRef.RegistersReload();
        }
    }

    async void SelectUserHandler(UserInfoModel? selected)
    {
        if (editDoc is null || selected?.UserId == recipientUser?.UserId)
            return;

        editDoc.RecipientIdentityUserId = selected?.UserId ?? "";
        if (string.IsNullOrWhiteSpace(editDoc.RecipientIdentityUserId))
            recipientUser = null;
        else
        {
            TResponseModel<UserInfoModel[]> getUsers = await IdentityRepo.GetUsersOfIdentityAsync([editDoc.RecipientIdentityUserId]);
            SnackBarRepo.ShowMessagesResponse(getUsers.Messages);
            if (getUsers.Success() && getUsers.Response is not null && getUsers.Response.Any(x => x.UserId == editDoc.RecipientIdentityUserId))
            {
                recipientUser = getUsers.Response.First(x => x.UserId == editDoc.RecipientIdentityUserId);
                await ReadRecipientUser();
            }
        }
        StateHasChanged();
    }

    async Task SetDeliveryAddressFromUserRecipient()
    {
        if (string.IsNullOrWhiteSpace(editDoc?.RecipientIdentityUserId))
            throw new Exception("editDoc?.RecipientIdentityUserId");

        await SetBusyAsync();
        TResponseModel<UserInfoModel[]> res = await IdentityRepo.GetUsersOfIdentityAsync([editDoc.RecipientIdentityUserId]);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success() && res.Response is not null && res.Response.Any(x => x.UserId == editDoc.RecipientIdentityUserId))
        {
            UserInfoModel _usr = res.Response.First(x => x.UserId == editDoc.RecipientIdentityUserId);
            editDoc.KladrCode = _usr.KladrCode;
            editDoc.KladrTitle = _usr.KladrTitle;
            editDoc.AddressUserComment = _usr.AddressUserComment;
        }
        await SetBusyAsync(false);
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

        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        await SetBusyAsync();
        if (editDoc.Id <= 0)
        {
            TResponseModel<int> res = await RetailRepo.CreateDeliveryDocumentAsync(new()
            {
                Payload = CreateDeliveryDocumentRetailRequestModel.Build(editDoc, InjectToOrder?.Id ?? 0),
                SenderActionUserId = CurrentUserSession.UserId,
            });
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            if (res.Success() && res.Response > 0)
                NavRepo.NavigateTo($"/retail/delivery-document/{res.Response}");
        }
        else
        {
            TResponseModel<Guid?> res = await RetailRepo.UpdateDeliveryDocumentAsync(new() { SenderActionUserId = CurrentUserSession.UserId, Payload = editDoc });
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success())
            {
                if (res.Response is not null)
                    editDoc.Version = res.Response.Value;

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

    async void ReloadDeliveriesOrdersLinksAction()
    {
        if (DeliveryDocumentId > 0)
        {
            await SetBusyAsync();

            TResponseModel<decimal> totalW = await RetailRepo.TotalWeightOrdersDocumentsLinksAsync(new() { DeliveryDocumentId = DeliveryDocumentId });
            SnackBarRepo.ShowMessagesResponse(totalW.Messages);
            totalWeightOrdersDocumentsLinks = totalW.Response;

            await SetBusyAsync(false);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{Routes.DELIVERY_CONTROLLER_NAME}/{Routes.DOCUMENT_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={Routes.IMAGE_ACTION_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={DeliveryDocumentId}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);
        
        TResponseModel<int?> res_RubricDeliveryTypePickup = await StorageTransmissionRepo.ReadParameterAsync<int?>(GlobalStaticCloudStorageMetadata.DeliveryTypePickup);
        rubricDeliveryTypePickup = res_RubricDeliveryTypePickup.Response ?? 0;

        string ctx = Path.Combine(Routes.DELIVERIES_CONTROLLER_NAME, Routes.TYPES_CONTROLLER_NAME);
        AllDeliveriesTypes = await RubricsRepo.RubricsChildListAsync(new() { ContextName = ctx });

        if (DeliveryDocumentId > 0)
        {
            TResponseModel<DeliveryDocumentRetailModelDB[]>? res = await RetailRepo.GetDeliveryDocumentsAsync(new() { Ids = [DeliveryDocumentId] });
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            if (res.Response is not null && res.Response.Length == 1)
            {
                currentDoc = res.Response.First();
                TResponseModel<decimal> totalW = await RetailRepo.TotalWeightOrdersDocumentsLinksAsync(new() { DeliveryDocumentId = currentDoc.Id });
                SnackBarRepo.ShowMessagesResponse(totalW.Messages);
                totalWeightOrdersDocumentsLinks = totalW.Response;
                await ReadRecipientUser();
            }
        }
        else
        {
            TResponseModel<int?> defaultWarehouse = await StorageTransmissionRepo.ReadParameterAsync<int?>(GlobalStaticCloudStorageMetadata.WarehouseDefaultForRetailDelivery);

            if (!string.IsNullOrWhiteSpace(ClientId))
            {
                TResponseModel<UserInfoModel[]> getUsers = await IdentityRepo.GetUsersOfIdentityAsync([ClientId]);
                SnackBarRepo.ShowMessagesResponse(getUsers.Messages);
                if (getUsers.Success() && getUsers.Response is not null && getUsers.Response.Any(x => x.UserId == ClientId))
                {
                    recipientUser = getUsers.Response.First(x => x.UserId == ClientId);
                }
            }

            currentDoc = new()
            {
                AuthorIdentityUserId = CurrentUserSession.UserId,
                RecipientIdentityUserId = ClientId ?? CurrentUserSession.UserId,
                WarehouseId = defaultWarehouse.Response ?? 0
            };
        }
        editDoc = GlobalTools.CreateDeepCopy(currentDoc);
        await SetBusyAsync(false);
    }

    async Task ReadRecipientUser()
    {
        if (recipientUser is null)
            return;

        if (recipientUser.TelegramId.HasValue)
        {
            List<ChatTelegramStandardModel> chats = await TelegramRepo.ChatsReadTelegramAsync([recipientUser.TelegramId.Value]);
            currentChatTelegram = chats.FirstOrDefault();
        }
        StateHasChangedCall();
    }
}