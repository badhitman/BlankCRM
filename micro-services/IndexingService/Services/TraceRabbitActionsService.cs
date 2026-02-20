////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using SharedLib;

namespace IndexingService;

/// <summary>
/// TraceRabbitActionsService
/// </summary>
public class TraceRabbitActionsService : ITraceRabbitActionsService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SaveActionAsync(TraceRabbitActionRequestModel req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
