////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorWebLib.Components.Commerce.OrderDocumentObject;

/// <summary>
/// OrderDocumentObjectComponent
/// </summary>
public partial class OrderDocumentObjectComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommRepo { get; set; } = default!;

    [Inject]
    IHelpdeskTransmission HelpdeskRepo { get; set; } = default!;

    [Inject]
    IStorageTransmission StorageRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;

    [Inject]
    IJSRuntime JsRuntimeRepo { get; set; } = default!;


    /// <summary>
    /// Document
    /// </summary>
    [Parameter, EditorRequired]
    public required OrderDocumentModelDB Document { get; set; }

    /// <summary>
    /// Issue
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required IssueHelpdeskModelDB Issue { get; set; }

    bool ShowingAttachmentsOrderArea;
    List<RubricIssueHelpdeskModelDB> currentWarehouses = default!;

    async Task OrderReport()
    {
        ArgumentNullException.ThrowIfNull(CurrentUserSession);
        TAuthRequestModel<int> req = new()
        {
            Payload = Document.Id,
            SenderActionUserId = CurrentUserSession.UserId
        };
        await SetBusyAsync();
        TResponseModel<FileAttachModel> res = await CommRepo.OrderReportGetAsync(req);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        if (res.Success() && res.Response is not null && res.Response.Data.Length != 0)
        {
            using MemoryStream ms = new(res.Response.Data);
            using DotNetStreamReference streamRef = new(stream: ms);
            await JsRuntimeRepo.InvokeVoidAsync("downloadFileFromStream", res.Response.Name, streamRef);
        }
    }

    async Task OrderToCart()
    {
        OrderDocumentModelDB doc = GlobalTools.CreateDeepCopy(Document)!;

        doc.Id = 0;
        doc.ExternalDocumentId = null;
        doc.CreatedAtUTC = DateTime.UtcNow;
        doc.LastAtUpdatedUTC = DateTime.UtcNow;
        doc.HelpdeskId = null;
        doc.Name = "Новый";
        doc.Information = null;

        doc.Organization = null;
        doc.OfficesTabs!.ForEach(x =>
        {
            x.Id = 0;
            //x.AddressOrganization = null;
            x.OrderId = 0;
            x.Rows?.ForEach(y =>
            {
                y.Id = 0;
                //y.OrderDocument = doc;
                y.OrderId = 0;
                //y.Goods = null;
                //y.Offer = null;
            });
        });


        await SetBusyAsync();

        TResponseModel<int> res = await StorageRepo.SaveParameterAsync(doc, GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession!.UserId), true);

        SnackbarRepo.ShowMessagesResponse(res.Messages);
        SnackbarRepo.Add("Содержимое документа отправлено в корзину для формирования нового заказа", Severity.Info, c => c.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

        if (res.Success())
            NavRepo.NavigateTo("/create-order");
        else
            IsBusyProgress = false;

    }

    async Task OrderNull()
    {
        TAuthRequestModel<StatusChangeRequestModel> req = new()
        {
            SenderActionUserId = CurrentUserSession!.UserId,
            Payload = new()
            {
                DocumentId = Issue.Id,
                Step = StatusesDocumentsEnum.Canceled,
            }
        };
        await SetBusyAsync();

        TResponseModel<bool> res = await HelpdeskRepo.StatusChangeAsync(req);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        if (res.Response && res.Success())
            NavRepo.ReloadPage();

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();
        int[] orderWarehouses = [.. Document.OfficesTabs!.Select(x => x.WarehouseId).Distinct()];
        await SetBusyAsync();

        TResponseModel<List<RubricIssueHelpdeskModelDB>> getWarehouses = await HelpdeskRepo.RubricsGetAsync(orderWarehouses);
        SnackbarRepo.ShowMessagesResponse(getWarehouses.Messages);
        currentWarehouses = getWarehouses.Response ?? [];

        TResponseModel<bool?> res = await StorageRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.ShowingAttachmentsOrderArea);
        if (!res.Success())
            SnackbarRepo.ShowMessagesResponse(res.Messages);

        ShowingAttachmentsOrderArea = res.Response == true;
        await SetBusyAsync(false);
    }
}