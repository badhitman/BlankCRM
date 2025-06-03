////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// IStockSharpBaseService
/// </summary>
public interface IStockSharpBaseService
{
    /// <summary>
    /// Проверка связи
    /// </summary>
    public Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default);
}