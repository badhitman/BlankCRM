////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ITraceRabbitActionsService
/// </summary>
public interface ITraceRabbitActionsService
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> SaveActionAsync(TraceRabbitActionRequestModel req, CancellationToken token = default);
}
