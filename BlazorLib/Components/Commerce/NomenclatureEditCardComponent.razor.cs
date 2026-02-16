////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// NomenclatureEditCardComponent
/// </summary>
public partial class NomenclatureEditCardComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IOptions<CommerceConfigModel> CommerceConf { get; set; } = default!;

    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// NomenclatureId
    /// </summary>
    [Parameter, EditorRequired]
    public int NomenclatureId { get; set; }

    /// <summary>
    /// ViewMode
    /// </summary>
    [Parameter]
    public string? ViewMode { get; set; }


    bool _isLoad;
    MudTabs? panelRef;
    string? activePrefixName;

    int _activeIndexWrapper;
    int WrapperActiveIndex
    {
        get => _activeIndexWrapper;
        set
        {
            _activeIndexWrapper = value;
            _isLoad = false;
        }
    }

    int _activeIndexGoodsRubrics;
    int GoodsRubricsActiveIndex
    {
        get => _activeIndexGoodsRubrics;
        set
        {
            _activeIndexGoodsRubrics = value;            
            activePrefixName = panelRef?.ActivePanel?.ID?.ToString();
        }
    }
    
    NomenclatureModelDB? originNomenclature, editNomenclature;

    OffersListModesEnum GetMode => string.IsNullOrWhiteSpace(ViewMode) || !Enum.TryParse(typeof(OffersListModesEnum), ViewMode, out object? pvm) ? OffersListModesEnum.Goods : (OffersListModesEnum)pvm;
    int[] SelectedNodesRead() => originNomenclature?.RubricsJoins?.Select(x => x.RubricId).ToArray() ?? [];

    async void SelectedRubricsChange(IReadOnlyCollection<RubricNestedModel?> req)
    {
        if (CurrentUserSession is null)
            return;
        if (!_isLoad)
        {
            _isLoad = true;
            return;
        }

        if (editNomenclature?.RubricsJoins is not null && !req.Any(x => !editNomenclature!.RubricsJoins.Any(y => y.RubricId == x?.Id)) && !editNomenclature.RubricsJoins.Any(x => !req.Any(y => y?.Id == x.RubricId)))
            return;

        await SetBusyAsync();
        ResponseBaseModel res = await CommerceRepo.RubricsForNomenclaturesSetAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                OwnerId = NomenclatureId,
                RubricsIds = [.. req.Select(x => x!.Id)],
            }
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await LoadNomenclatureData();
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadNomenclatureData();
    }
    
    async Task LoadNomenclatureData()
    {
        if (CurrentUserSession is null)
            return;

        await SetBusyAsync();
        TResponseModel<List<NomenclatureModelDB>> res = await CommerceRepo.NomenclaturesReadAsync(new() { SenderActionUserId = CurrentUserSession.UserId, Payload = [NomenclatureId] });
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Response is null)
            throw new Exception($"res.Response is null > {nameof(LoadNomenclatureData)}");

        originNomenclature = res.Response.Single();
        editNomenclature = GlobalTools.CreateDeepCopy(originNomenclature) ?? throw new Exception($"editNomenclature is null > {nameof(LoadNomenclatureData)}");

        await SetBusyAsync(false);
    }
}