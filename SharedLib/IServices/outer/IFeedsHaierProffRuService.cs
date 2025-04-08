////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IFeedsHaierProffRuService
/// </summary>
public interface IFeedsHaierProffRuService : IOuterApiService
{
    /// <summary>
    /// Получение товаров из rss/feed
    /// </summary>
    public Task<TResponseModel<List<FeedItemHaierModel>>> ProductsFeedGetAsync(CancellationToken token = default);
}