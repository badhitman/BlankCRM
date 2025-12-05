////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorLib.Components.Commerce.OrderDocumentObject;

/// <summary>
/// OrderDocumentObjectComponent
/// </summary>
public partial class OrderDocumentObjectComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommRepo { get; set; } = default!;

    [Inject]
    IHelpDeskTransmission HelpDeskRepo { get; set; } = default!;


    [Inject]
    IRubricsTransmission RubricsRepo { get; set; } = default!;


    [Inject]
    IParametersStorageTransmission StorageRepo { get; set; } = default!;

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
    public required IssueHelpDeskModelDB Issue { get; set; }

    bool ShowingAttachmentsOrderArea;
    List<RubricStandardModel> currentWarehouses = default!;

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
        SnackBarRepo.ShowMessagesResponse(res.Messages);
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
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        OrderDocumentModelDB doc = GlobalTools.CreateDeepCopy(Document)!;

        doc.Id = 0;
        doc.ExternalDocumentId = null;
        doc.CreatedAtUTC = DateTime.UtcNow;
        doc.LastUpdatedAtUTC = DateTime.UtcNow;
        doc.HelpDeskId = null;
        doc.Name = "Новый";
        doc.Description = null;

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

        TResponseModel<int> res = await StorageRepo.SaveParameterAsync(doc, GlobalStaticCloudStorageMetadata.OrderCartForUser(CurrentUserSession.UserId), true);

        SnackBarRepo.ShowMessagesResponse(res.Messages);
        SnackBarRepo.Add("Содержимое документа отправлено в корзину для формирования нового заказа", Severity.Info, c => c.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);

        if (res.Success())
            NavRepo.NavigateTo("/create-order");
        else
            await SetBusyAsync(false);

    }

    async Task OrderNull()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        TAuthRequestModel<StatusChangeRequestModel> req = new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                DocumentId = Issue.Id,
                Step = StatusesDocumentsEnum.Canceled,
            }
        };
        await SetBusyAsync();

        TResponseModel<bool> res = await HelpDeskRepo.StatusChangeAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
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

        TResponseModel<List<RubricStandardModel>> getWarehouses = await RubricsRepo.RubricsGetAsync(orderWarehouses);
        SnackBarRepo.ShowMessagesResponse(getWarehouses.Messages);
        currentWarehouses = getWarehouses.Response ?? [];

        TResponseModel<bool?> res = await StorageRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.ShowingAttachmentsOrderArea);
        if (!res.Success())
            SnackBarRepo.ShowMessagesResponse(res.Messages);

        ShowingAttachmentsOrderArea = res.Response == true;
        await SetBusyAsync(false);
    }
}