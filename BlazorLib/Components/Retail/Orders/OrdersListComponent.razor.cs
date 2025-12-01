////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Orders;

/// <summary>
/// OrdersListComponent
/// </summary>
public partial class OrdersListComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission HelpDeskRepo { get; set; } = default!;

    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public string? FilterClientId { get; set; }

    private bool _visible;
    private readonly DialogOptions _dialogOptions = new() { FullWidth = true };


    private void Submit() => _visible = false;

    void CreateNewOrderOpenDialog()
    {
        _visible = true;
    }

    /// <summary>
    /// RubricsCache
    /// </summary>
    protected List<RubricStandardModel> RubricsCache = [];
    /// <summary>
    /// UsersCache
    /// </summary>
    protected List<UserInfoModel> UsersCache = [];


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
        await SetBusyAsync(token: token);
        TPaginationResponseModel<RetailDocumentModelDB>? res = await RetailRepo.SelectRetailDocumentsAsync(new(), token);

        if (res.Response is not null)
        {
            IEnumerable<string> _usersIds = res.Response
                .Select(x => x.AuthorIdentityUserId)
                .Union(res.Response.Select(x => x.BuyerIdentityUserId))
                .Distinct();

            IEnumerable<int> _rubricsIds = res.Response.Select(x=>x.WarehouseId).Distinct();

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