////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Warehouse;

/// <summary>
/// WarehouseMainComponent
/// </summary>
public partial class WarehouseMainComponent : BlazorBusyComponentRubricsCachedModel
{
    MudTable<WarehouseDocumentModelDB>? table;

    string? searchString = null;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    async Task<TableData<WarehouseDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req = new()
        {
            Payload = new()
            {
                SearchQuery = searchString,
                IncludeExternalData = true,
            },
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };
        TPaginationResponseModel<WarehouseDocumentModelDB> rest = await CommerceRepo.WarehouseDocumentsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);

        if (rest.Response is not null)
        {
            await CacheRubricsUpdate(rest.Response.Select(x => x.WarehouseId).Union(rest.Response.Select(x => x.WritingOffWarehouseId)));
            return new TableData<WarehouseDocumentModelDB>() { TotalItems = rest.TotalRowsCount, Items = rest.Response };
        }

        await SetBusyAsync(false, token: token);
        return new TableData<WarehouseDocumentModelDB>() { TotalItems = 0, Items = [] };
    }

    string WarehouseTitle(WarehouseDocumentModelDB context)
    {
        string res = RubricsCache.FirstOrDefault(x => x.Id == context.WarehouseId)?.Name ?? context.WarehouseId.ToString();

        if (context.WritingOffWarehouseId > 0)
            res = $"{RubricsCache.FirstOrDefault(x => x.Id == context.WritingOffWarehouseId)?.Name ?? context.WritingOffWarehouseId.ToString()} -> {res}";
       
        return res;
    }

    void OnSearch(string text)
    {
        searchString = text;
        table?.ReloadServerData();
    }
}