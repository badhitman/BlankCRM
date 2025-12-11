////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Orders;

/// <summary>
/// RetailOrdersListComponent
/// </summary>
public partial class RetailOrdersListComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission HelpDeskRepo { get; set; } = default!;

    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }

    /// <summary>
    /// Вывод заказов только для указанного документа доставки
    /// </summary>
    [Parameter]
    public int? FilterDeliveryId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public Action<TableRowClickEventArgs<RetailDocumentModelDB>>? RowClickEventHandler { get; set; }


    /// <summary>
    /// RubricsCache
    /// </summary>
    protected List<RubricStandardModel> RubricsCache = [];
    /// <summary>
    /// UsersCache
    /// </summary>
    protected List<UserInfoModel> UsersCache = [];

    MudTable<RetailDocumentModelDB>? tableRef;
    bool _visibleCreateNewOrder;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true
    };


    void CreateNewOrderOpenDialog()
    {
        _visibleCreateNewOrder = true;
    }

    void RowClickEvent(TableRowClickEventArgs<RetailDocumentModelDB> tableRowClickEventArgs)
    {
        if (RowClickEventHandler is not null)
            RowClickEventHandler(tableRowClickEventArgs);
    }


    int? initDeleteDeliveryFromOrder;
    async Task DeleteOrderLink(int orderDocumentId)
    {
        if (initDeleteDeliveryFromOrder is null)
        {
            initDeleteDeliveryFromOrder = orderDocumentId;
            return;
        }
        initDeleteDeliveryFromOrder = null;

        if (!FilterDeliveryId.HasValue || FilterDeliveryId <= 0)
        {
            SnackBarRepo.Error("Не определён контекст заказа (розница)");
            StateHasChanged();
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel res = await RetailRepo.DeleteDeliveryOrderLinkDocumentAsync(new()
        {
            DeliveryId = FilterDeliveryId.Value,
            OrderId = orderDocumentId,
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }


    /// <summary>
    /// CacheUsersUpdate
    /// </summary>
    protected async Task CacheUsersUpdate(string[] usersIds, CancellationToken token)
    {
        usersIds = [.. usersIds.Where(x => !string.IsNullOrWhiteSpace(x) && !UsersCache.Any(y => y.UserId == x)).Distinct()];
        if (usersIds.Length == 0)
            return;

        TResponseModel<UserInfoModel[]> users = await IdentityRepo.GetUsersOfIdentityAsync(usersIds, token);
        SnackBarRepo.ShowMessagesResponse(users.Messages);
        if (users.Success() && users.Response is not null && users.Response.Length != 0)
            lock (UsersCache)
            {
                UsersCache.AddRange(users.Response.Where(x => !UsersCache.Any(y => y.UserId == x.UserId)));
            }
    }

    /// <summary>
    /// CacheRubricsUpdate
    /// </summary>
    protected async Task CacheRubricsUpdate(IEnumerable<int> rubricsIds, CancellationToken token)
    {
        rubricsIds = rubricsIds.Where(x => x > 0 && !RubricsCache.Any(y => y.Id == x)).Distinct();
        if (!rubricsIds.Any())
            return;

        TResponseModel<List<RubricStandardModel>> rubrics = await HelpDeskRepo.RubricsGetAsync(rubricsIds, token);
        SnackBarRepo.ShowMessagesResponse(rubrics.Messages);
        if (rubrics.Success() && rubrics.Response is not null && rubrics.Response.Count != 0)
            lock (RubricsCache)
            {
                RubricsCache.AddRange(rubrics.Response.Where(x => !RubricsCache.Any(y => y.Id == x.Id)));
            }
    }

    async Task<TableData<RetailDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel> req = new() { Payload = new() };

        if (!string.IsNullOrWhiteSpace(ClientId))
            req.Payload.BuyersFilterIdentityId = [ClientId];

        if (FilterDeliveryId.HasValue && FilterDeliveryId > 0)
            req.Payload.FilterDeliveryId = FilterDeliveryId.Value;

        await SetBusyAsync(token: token);
        TPaginationResponseModel<RetailDocumentModelDB> res = await RetailRepo.SelectRetailDocumentsAsync(req, token);

        if (res.Response is not null)
        {
            IEnumerable<string> _usersIds = res.Response
                .Select(x => x.AuthorIdentityUserId)
                .Union(res.Response.Select(x => x.BuyerIdentityUserId))
                .Distinct();

            IEnumerable<int> _rubricsIds = res.Response.Select(x => x.WarehouseId).Distinct();

            List<Task> tasks = [
                Task.Run(async () => { await CacheUsersUpdate([.._usersIds],token); }, token),
                Task.Run(async () => { await CacheRubricsUpdate([.. _rubricsIds], token); }, token),
            ];

            await Task.WhenAll(tasks);
        }

        await SetBusyAsync(false, token: token);
        return new TableData<RetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}