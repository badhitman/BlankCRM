////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RSS feed HaierProff
/// </summary>
public interface IFeedsHaierProffRuService : IOuterApiService
{
    /// <summary>
    /// Получение товаров из rss/feed
    /// </summary>
    public Task<TResponseModel<List<FeedItemHaierModel>>> ProductsFeedGetAsync(CancellationToken token = default);
}