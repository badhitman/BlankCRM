////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Articles (service)
/// </summary>
public interface IArticlesService
{
    /// <summary>
    /// Установит рубрики для статьи
    /// </summary>
    public Task<TResponseModel<bool>> UpdateRubricsForArticle(ArticleRubricsSetModel req, CancellationToken token = default);

    /// <summary>
    /// Создать/обновить статью
    /// </summary>
    public Task<TResponseModel<int>> ArticleCreateOrUpdate(ArticleModelDB art, CancellationToken token = default);

    /// <summary>
    /// Подбор статей
    /// </summary>
    public Task<TPaginationResponseModel<ArticleModelDB>> ArticlesSelect(TPaginationRequestModel<SelectArticlesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Получить статьи
    /// </summary>
    public Task<TResponseModel<ArticleModelDB[]>> ArticlesRead(int[] req, CancellationToken token = default);
}