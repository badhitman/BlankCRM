////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// Журнал заказов
/// </summary>
public partial class OrdersJournalComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;

    [Inject]
    IHelpDeskTransmission HelpDeskRepo { get; set; } = default!;


    /// <summary>
    /// Фильтр по организации
    /// </summary>
    [Parameter]
    public int? OrganizationFilter { get; set; }

    /// <summary>
    /// Фильтр по адресу организации
    /// </summary>
    [Parameter]
    public int AddressForOrganization { get; set; } = default!;

    /// <summary>
    /// Фильтр по номенклатуре
    /// </summary>
    [Parameter]
    public int? NomenclatureFilter { get; set; }

    /// <summary>
    /// Фильтр по торговому/коммерческому предложению
    /// </summary>
    [Parameter]
    public int? OfferFilter { get; set; }


    MudTable<OrderDocumentModelDB>? tableRef;
    List<OrderDocumentModelDB> documentsPartData = [];
    readonly List<IssueHelpDeskModelDB> IssuesCacheDump = [];

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (tableRef is not null)
            await tableRef.ReloadServerData();
    }

    async Task UpdateCacheIssues()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        IEnumerable<int> q = documentsPartData
            .Where(x => x.HelpDeskId.HasValue && x.HelpDeskId.Value > 0)
            .Select(x => x.HelpDeskId!.Value);

        if (!q.Any())
            return;

        TResponseModel<IssueHelpDeskModelDB[]> res = await HelpDeskRepo.IssuesReadAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                IssuesIds = [.. q],
            }
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Response is not null)
            IssuesCacheDump.AddRange(res.Response);
    }

    async Task<TableData<OrderDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        if (CurrentUserSession is null)
            return new TableData<OrderDocumentModelDB>() { TotalItems = 0, Items = [] };

        TPaginationRequestStandardModel<TAuthRequestStandardModel<OrdersSelectRequestModel>> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
            Payload = new()
            {
                SenderActionUserId = CurrentUserSession.UserId,
                Payload = new()
                {
                    IncludeExternalData = true,
                    OrganizationFilter = OrganizationFilter,
                    AddressForOrganizationFilter = AddressForOrganization,
                    NomenclatureFilter = NomenclatureFilter.HasValue ? [NomenclatureFilter.Value] : null,
                    OfferFilter = OfferFilter.HasValue ? [OfferFilter.Value] : null,
                }
            }
        };

        await SetBusyAsync(token: token);

        TPaginationResponseStandardModel<OrderDocumentModelDB> res = await CommerceRepo.OrdersSelectAsync(req, token);

        if (res.Response is null)
        {
            await SetBusyAsync(false, token);
            return new TableData<OrderDocumentModelDB>() { TotalItems = 0, Items = [] };
        }
        documentsPartData = res.Response;
        await UpdateCacheIssues();
        await SetBusyAsync(false, token);
        return new TableData<OrderDocumentModelDB>()
        {
            TotalItems = res.TotalRowsCount,
            Items = documentsPartData
        };
    }
}