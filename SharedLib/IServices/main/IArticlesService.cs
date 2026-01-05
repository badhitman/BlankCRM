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
    public Task<TResponseModel<bool>> UpdateRubricsForArticleAsync(ArticleRubricsSetModel req, CancellationToken token = default);

    /// <summary>
    /// Создать/обновить статью
    /// </summary>
    public Task<TResponseModel<int>> ArticleCreateOrUpdateAsync(ArticleModelDB art, CancellationToken token = default);

    /// <summary>
    /// Подбор статей
    /// </summary>
    public Task<TPaginationResponseStandardModel<ArticleModelDB>> ArticlesSelectAsync(TPaginationRequestStandardModel<SelectArticlesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Получить статьи
    /// </summary>
    public Task<TResponseModel<ArticleModelDB[]>> ArticlesReadAsync(int[] req, CancellationToken token = default);
}