////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using MudBlazor;
using SharedLib;

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
    List<RootKLADRModelDB> SelectionProgressSteps = [];

    /// <summary>
    /// Выбранный объект
    /// </summary>
    public RootKLADRModelDB? SelectedObject => SelectionProgressSteps.LastOrDefault();

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
            InvokeAsync(RebuildTable);
        }
    }

    List<KladrResponseModel>? partData;

    string[] _states = [];
    bool hideTable;
    async Task RebuildTable()
    {
        hideTable = true;
        StateHasChanged();
        await Task.Delay(1);
        partData = null;
        hideTable = false;
        StateHasChanged();
        await Task.Delay(1);
    }

    void SelectRowAction(KladrResponseModel selected)
    {
        List<RootKLADRModelDB>? parents = selected.Parents?.Skip(1 + SelectionProgressSteps.Count).ToList();
        if (parents is not null && parents.Count != 0)
            SelectionProgressSteps.AddRange(parents);

        SelectionProgressSteps.Add(selected.Payload.ToObject<RootKLADRModelDB>()!);
        StateHasChanged();
        if (selected.Chain == KladrChainTypesEnum.StreetsInPopPoint)
            MudDialog.Close(DialogResult.Ok(true));
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    private async Task<TableData<KladrResponseModel>> ServerReload(TableState state, CancellationToken token)
    {
        if (CurrentRegion is null || string.IsNullOrWhiteSpace(FindName))
            return new TableData<KladrResponseModel>() { TotalItems = 0, Items = [] };

        string _codeLikeFilter = $"{CurrentRegion.Code[..2]}%00";
        RootKLADRModelDB? sObj = SelectedObject;
        
        if(sObj is not null)
        {
          var  MetaData = CodeKladrModel.Build(sObj.CODE);
        }

        KladrFindRequestModel req = new()
        {
            CodeLikeFilter = [_codeLikeFilter],
            FindText = $"%{FindName}%",
            PageNum = state.Page,
            PageSize = state.PageSize,
        };
        await SetBusy(token: token);
        TPaginationResponseModel<KladrResponseModel> res = await kladrRepo.ObjectsFindAsync(req, token);
        partData = res.Response;
        await SetBusy(false, token: token);
        // Return the data
        return new TableData<KladrResponseModel>() { TotalItems = res.TotalRowsCount, Items = partData };
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusy();
        Dictionary<KladrChainTypesEnum, JObject[]> regionsRest = await kladrRepo.ObjectsListForParentAsync(new());
        await SetBusy(false);
        foreach (RootKLADRModel? region in regionsRest.SelectMany(x => x.Value).Select(x => x.ToObject<RootKLADRModel>()!))
            regions.Add(new(region.CODE, region.NAME, region.SOCR));

        _states = [.. regions.Where(x => x.Code.EndsWith("00")).Select(x => x.ToString()).Order()];
    }

    private Task<IEnumerable<string?>> SearchRegion(string? value, CancellationToken token)
    {
        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return Task.FromResult(_states.AsEnumerable())!;

        return Task.FromResult(_states.Where(x => x.Contains(value, StringComparison.OrdinalIgnoreCase)))!;
    }

    private void Cancel() => MudDialog.Cancel();
}