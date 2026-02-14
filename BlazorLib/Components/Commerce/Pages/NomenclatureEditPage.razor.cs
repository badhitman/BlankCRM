////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Commerce.Pages;

/// <summary>
/// NomenclatureEditPage
/// </summary>
public partial class NomenclatureEditPage : BlazorBusyComponentBaseAuthModel
{
    /// <summary>
    /// NomenclatureId
    /// </summary>
    [Parameter]
    public int NomenclatureId { get; set; }

    /// <summary>
    /// ViewMode
    /// </summary>
    [Parameter]
    public string? ViewMode {  get; set; }

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
}