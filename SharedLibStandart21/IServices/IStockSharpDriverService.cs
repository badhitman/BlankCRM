////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// Драйвер StockSharp (NET6)
/// </summary>
public interface IStockSharpDriverService : IStockSharpBaseService
{
    /// <summary>
    /// Connect
    /// </summary>
    public Task<ResponseBaseModel> Connect(CancellationToken? cancellationToken = default);

    /// <summary>
    /// Disconnect
    /// </summary>
    public Task<ResponseBaseModel> Disconnect(CancellationToken? cancellationToken = default);
}