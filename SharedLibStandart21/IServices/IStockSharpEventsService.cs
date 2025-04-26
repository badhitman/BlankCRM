////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// StockSharp - события
/// </summary>
public interface IStockSharpEventsService
{
    /// <summary>
    /// Security changed.
    /// </summary>
    public Task<ResponseBaseModel> ValuesChanged(ConnectorValuesChangedEventPayloadModel req, CancellationToken cancellationToken = default);
}