﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using SharedLib;
using System.Globalization;

namespace BlazorLib.Components.Kladr.control;

/// <summary>
/// KladrFindTableResultComponent
/// </summary>
public partial class KladrFindTableResultComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IKladrNavigationService kladrRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public string? FindText { get; set; }


    List<KladrResponseModel>? PartData;

    

    IEnumerable<string> _regionsSelected = [];
    
    MudTable<KladrResponseModel>? tableRef;
    List<RootKLADREquatableModel> regions = [];

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        Dictionary<KladrChainTypesEnum, JObject[]> regionsRest = await kladrRepo.ObjectsListForParent(new());
        foreach (RootKLADRModel? region in regionsRest.SelectMany(x => x.Value).Select(x => x.ToObject<RootKLADRModel>()!))
            regions.Add(new(region.CODE, region.NAME, region.SOCR));
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<KladrResponseModel>> ServerReload(TableState state, CancellationToken token)
    {
        KladrFindRequestModel req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            FindText = $"%{FindText}%",
            CodeLikeFilter = _regionsSelected?.Select(x => $"{x[..2]}%").ToArray(),
        };

        await SetBusy(token: token);
        TPaginationResponseModel<KladrResponseModel> res = await kladrRepo.ObjectsFind(req);
        await SetBusy(false, token: token);
        PartData = res.Response ?? [];
        return new TableData<KladrResponseModel>()
        {
            TotalItems = res.TotalRowsCount,
            Items = PartData
        };
    }

    private string GetMultiSelectionText(List<string> selectedValues)
    {
        return $"Выбран{(selectedValues.Count > 1 ? "о" : "")}: {string.Join(", ", selectedValues.Select(x => regions.First(y => y.Code == x).ToString()))}";
    }
}