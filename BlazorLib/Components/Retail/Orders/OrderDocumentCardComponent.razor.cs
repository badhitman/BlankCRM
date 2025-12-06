////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Orders;

/// <summary>
/// OrderDocumentCardComponent
/// </summary>
public partial class OrderDocumentCardComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission RubricsRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public int OrderId { get; set; }

    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }


    RetailDocumentModelDB? currentDocument, editDocument;
    UserInfoModel? authorUser, buyerUser;
    RubricStandardModel? currentWarehouse;
    OrderTableRowsComponent? tableRowsRef;

    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;

    DateTime? datePayment;
    DateTime? DatePayment
    {
        get => datePayment;
        set
        {
            if (editDocument is null)
                return;

            datePayment = value ?? DateTime.Now;
            editDocument.DateDocument = datePayment ?? DateTime.Now;
        }
    }

    bool SelectUserHandlerIsProgress;
    async Task SelectUserHandlerBusy(bool is_busy = true, CancellationToken token = default)
    {
        SelectUserHandlerIsProgress = is_busy;
        try
        {
            await Task.Delay(1, token);
        }
        catch
        {

        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Деактивация/отключение кнопки сохранения документа
    /// </summary>
    bool CannotSave
    {
        get
        {
            if (currentDocument is null || editDocument is null)
                return true;

            if (string.IsNullOrWhiteSpace(editDocument.BuyerIdentityUserId) || IsBusyProgress)
                return true;

            if (editDocument.WarehouseId < 1)
                return true;

            if (OrderId > 0)
                return
                    currentDocument.BuyerIdentityUserId == editDocument.BuyerIdentityUserId &&
                    currentDocument.WarehouseId == editDocument.WarehouseId &&
                    currentDocument.Name == editDocument.Name &&
                    currentDocument.StatusDocument == editDocument.StatusDocument &&
                    currentDocument.ExternalDocumentId == editDocument.ExternalDocumentId &&
                    currentDocument.Description == editDocument.Description &&
                    currentDocument.HelpDeskId == editDocument.HelpDeskId;

            return false;
        }
    }

    async Task SaveDocument()
    {
        if (editDocument is null)
            throw new ArgumentNullException(nameof(editDocument));

        await SetBusyAsync();
        if (OrderId < 1)
        {
            TResponseModel<int> res = await RetailRepo.CreateRetailDocumentAsync(editDocument);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success() && res.Response > 0)
                NavRepo.NavigateTo($"/retail/order-document/{res.Response}");
        }
        else
        {
            ResponseBaseModel res = await RetailRepo.UpdateRetailDocumentAsync(editDocument);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success())
                NavRepo.NavigateTo($"/retail/order-document/{OrderId}", true);
        }
        await SetBusyAsync(false);
    }

    void WarehouseSelectAction(UniversalBaseModel? selectedWarehouse)
    {
        if (editDocument is null)
            throw new ArgumentNullException(nameof(editDocument));

        editDocument.WarehouseId = selectedWarehouse?.Id ?? 0;

        StateHasChanged();
        tableRowsRef?.UpdateData();
    }

    async void SelectUserHandler(UserInfoModel? selected)
    {
        if (editDocument is null || selected?.UserId == buyerUser?.UserId)
            return;

        editDocument.BuyerIdentityUserId = selected?.UserId ?? "";
        if (string.IsNullOrWhiteSpace(editDocument.BuyerIdentityUserId))
            buyerUser = null;
        else
        {
            await SelectUserHandlerBusy();
            TResponseModel<UserInfoModel[]> getUsers = await IdentityRepo.GetUsersOfIdentityAsync([editDocument.BuyerIdentityUserId]);
            SnackBarRepo.ShowMessagesResponse(getUsers.Messages);
            if (getUsers.Success() && getUsers.Response is not null && getUsers.Response.Any(x => x.UserId == editDocument.BuyerIdentityUserId))
            {
                buyerUser = getUsers.Response.First(x => x.UserId == editDocument.BuyerIdentityUserId);
            }
            await SelectUserHandlerBusy(false);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{Routes.RETAIL_CONTROLLER_NAME}/{Routes.DOCUMENT_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={Routes.IMAGE_ACTION_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={OrderId}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);

        if (OrderId > 0)
        {
            await SetBusyAsync();
            TResponseModel<RetailDocumentModelDB[]> res = await RetailRepo.RetailDocumentsGetAsync(new() { Ids = [OrderId] });
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            if (res.Response is not null && res.Response.Length == 1)
            {
                currentDocument = res.Response.First();
                datePayment = currentDocument.DateDocument;
                List<Task> tasks = [
                        Task.Run(async () => {
                            TResponseModel<UserInfoModel[]> getUsers = await IdentityRepo.GetUsersOfIdentityAsync([currentDocument.BuyerIdentityUserId, currentDocument.AuthorIdentityUserId]);
                            SnackBarRepo.ShowMessagesResponse(getUsers.Messages);
                            if(getUsers.Success() && getUsers.Response is not null && getUsers.Response.Length == 2)
                            {
                                authorUser = getUsers.Response.First(x=>x.UserId == currentDocument.AuthorIdentityUserId);
                                buyerUser = getUsers.Response.First(x => x.UserId == currentDocument.BuyerIdentityUserId);
                            }
                        })
                    ];

                if (currentDocument.WarehouseId > 0)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        TResponseModel<List<RubricStandardModel>> getRubric = await RubricsRepo.RubricsGetAsync([currentDocument.WarehouseId]);
                        SnackBarRepo.ShowMessagesResponse(getRubric.Messages);
                        if (getRubric.Success() && getRubric.Response is not null && getRubric.Response.Count == 1)
                        {
                            currentWarehouse = getRubric.Response.First();
                        }
                    }));
                }

                await Task.WhenAll(tasks);
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
                    buyerUser = getUsers.Response.First(x => x.UserId == ClientId);
                }
            }

            currentDocument = new()
            {
                DateDocument = DateTime.Now,
                Rows = [],
                AuthorIdentityUserId = CurrentUserSession.UserId,
                BuyerIdentityUserId = ClientId ?? CurrentUserSession.UserId,
            };
            datePayment = currentDocument.DateDocument;
        }
        editDocument = GlobalTools.CreateDeepCopy(currentDocument);
    }
}