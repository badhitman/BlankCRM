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


    MudTabs? panelRef;
    string? activePrefixName;
    int _activeIndex;
    int ActiveIndex
    {
        get => _activeIndex;
        set
        {
            _activeIndex = value;
            activePrefixName = panelRef?.ActivePanel?.ID?.ToString();
        }
    }

    OffersListModesEnum GetMode => string.IsNullOrWhiteSpace(ViewMode) || !Enum.TryParse(typeof(OffersListModesEnum), ViewMode, out object? pvm) ? OffersListModesEnum.Goods : (OffersListModesEnum)pvm;
    int[] SelectedNodesRead() => [];//orignArticle?.RubricsJoins?.Select(x => x.RubricId).ToArray() ?? [];

    async void SelectedRubricsChange(IReadOnlyCollection<UniversalBaseModel?> req)
    {
        //if (editArticle?.RubricsJoins is not null && !req.Any(x => !editArticle!.RubricsJoins.Any(y => y.RubricId == x?.Id)) && !editArticle.RubricsJoins.Any(x => !req.Any(y => y?.Id == x.RubricId)))
        //    return;

        //await SetBusyAsync();
        //ResponseBaseModel res = await ArticlesRepo.UpdateRubricsForArticleAsync(new() { ArticleId = ArticleId, RubricsIds = [.. req.Select(x => x!.Id)] });
        //await LoadArticleData();
        //await SetBusyAsync(false);
        //SnackBarRepo.ShowMessagesResponse(res.Messages);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}