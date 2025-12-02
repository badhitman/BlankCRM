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


    /// <inheritdoc/>
    [Parameter]
    public int OrderId { get; set; }


    RetailDocumentModelDB? currentDocument;
    UserInfoModel? authorUser, buyerUser;
    RubricStandardModel? currentWarehouse;

    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;


    void WarehouseSelectAction(UniversalBaseModel? selectedWarehouse)
    {
        if (currentDocument is null)
            throw new ArgumentNullException(nameof(currentDocument));

        currentDocument.WarehouseId = selectedWarehouse?.Id ?? 0;

        StateHasChanged();
    }


    
     void SelectUserHandler(UserInfoModel? selected)
    {
        if (currentDocument is null)
            return;

        currentDocument.BuyerIdentityUserId = selected?.UserId ?? "";
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
            currentDocument = new()
            {
                AuthorIdentityUserId = CurrentUserSession.UserId,
                BuyerIdentityUserId = CurrentUserSession.UserId,
            };
    }
}