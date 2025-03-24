////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using MudBlazor;
using SharedLib;
using static MudBlazor.CategoryTypes;
using System.Net.Http;

namespace BlazorLib.Components.Kladr.control.input;

/// <summary>
/// KladrSelectDialogComponent
/// </summary>
public partial class KladrSelectDialogComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IKladrNavigationService kladrRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required IMudDialogInstance MudDialog { get; set; }


    RootKLADREquatableModel? CurrentRegion;
    List<RootKLADREquatableModel> regions = [];
    MudDataGrid<RootKLADREquatableModel>? dataGridRef;
    List<KladrResponseModel>? PartData;


    string? _selectedRegionName;
    string? SelectedRegionName
    {
        get => _selectedRegionName;
        set
        {
            _selectedRegionName = value;
            CurrentRegion = regions.First(x => x.ToString().Equals(_selectedRegionName));
        }
    }

    string? _findName;
    string? FindName
    {
        get => _findName;
        set
        {
            _findName = value;
            if (dataGridRef is not null)
                InvokeAsync(dataGridRef.ReloadServerData);
        }
    }




    private string[] _states = [];

    private async Task<IEnumerable<string?>> SearchRegion(string? value, CancellationToken token)
    {
        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return _states;

        return _states.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    private async Task<TableData<KladrResponseModel>> ServerReload(TableState state, CancellationToken token)
    {
        if (CurrentRegion is null || string.IsNullOrWhiteSpace(FindName))
            return new TableData<KladrResponseModel>() { TotalItems = 0, Items = [] };

        KladrFindRequestModel req = new()
        {
            CodeLikeFilter = [$"{CurrentRegion.Code[..2]}%00"],
            FindText = FindName,
            PageNum = state.Page,
            PageSize = state.PageSize,
        };
        await SetBusy(token: token);
        TPaginationResponseModel<KladrResponseModel> res = await kladrRepo.ObjectsFind(req, token);
        await SetBusy(false, token: token);
        // Return the data
        return new TableData<KladrResponseModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusy();
        Dictionary<KladrChainTypesEnum, JObject[]> regionsRest = await kladrRepo.ObjectsListForParent(new());
        await SetBusy(false);
        foreach (RootKLADRModel? region in regionsRest.SelectMany(x => x.Value).Select(x => x.ToObject<RootKLADRModel>()!))
            regions.Add(new(region.CODE, region.NAME, region.SOCR));

        _states = [.. regions.Where(x => x.Code.EndsWith("00")).Select(x => x.ToString()).Order()];
    }

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();
}