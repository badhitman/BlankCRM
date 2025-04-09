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
    /// остатки на складах
    /// </summary>
    public Task<TResponseModel<List<BreezRuLeftoverModel>>> LeftoversGetAsync(string? nc = null, CancellationToken token = default);
}