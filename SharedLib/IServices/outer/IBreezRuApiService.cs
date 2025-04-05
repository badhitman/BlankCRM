////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Breez.ru api
/// </summary>
public interface IBreezRuApiService : IOuterApiBaseService
{
    /// <summary>
    /// остатки на доступных складах
    /// </summary>
    public Task<TResponseModel<List<BreezRuGoodsModel>>> LeftoversGetAsync(CancellationToken token = default);
}