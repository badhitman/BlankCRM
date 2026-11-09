////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ITraceRabbitActionsService RequiredQueues
/// </summary>
public interface ITraceRabbitActionsService
{
    /// <summary>
    /// Перечень очередей, которые следует подробно фиксировать (сохранять все этапы обработки каждого вызова)
    /// </summary>
    public static string[]? TracesRequiredQueues { get; set; }

    /// <inheritdoc/>
    public Task<ResponseBaseModel> SaveActionAsync(TraceRabbitActionRequestModel req, CancellationToken token = default);
}
