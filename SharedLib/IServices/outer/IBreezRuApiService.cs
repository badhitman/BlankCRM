////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading;

namespace SharedLib;

/// <summary>
/// Breez.ru api
/// </summary>
public interface IBreezRuApiService : IOuterApiService
{
    /// <summary>
    /// остатки на доступных складах
    /// </summary>
    public Task<TResponseModel<List<BreezRuGoodsModel>>> LeftoversGetAsync(string? nc = null, CancellationToken token = default);
}