////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// Драйвер StockSharp (NET6)
/// </summary>
public interface IDriverStockSharpService : IStockSharpBaseService
{
    /// <summary>
    /// CanConnect
    /// </summary>
    public Task<AboutConnectResponseModel> AboutConnect(CancellationToken? cancellationToken = default);

    /// <summary>
    /// Connect
    /// </summary>
    public Task<ResponseBaseModel> Connect(CancellationToken? cancellationToken = default);

    /// <summary>
    /// Disconnect
    /// </summary>
    public Task<ResponseBaseModel> Disconnect(CancellationToken? cancellationToken = default);

    /// <summary>
    /// OrderRegister
    /// </summary>
    public Task<ResponseBaseModel> OrderRegisterAsync(CreateOrderRequestModel req, CancellationToken cancellationToken = default);
}