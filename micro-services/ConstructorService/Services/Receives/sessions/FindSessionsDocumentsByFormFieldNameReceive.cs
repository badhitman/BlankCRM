////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Найти порцию сессий по имени поля (с пагинацией)
/// </summary>
public class FindSessionsDocumentsByFormFieldNameReceive(IConstructorService conService) : IResponseReceive<FormFieldModel?, TResponseModel<EntryDictModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FindSessionsDocumentsByFormFieldNameReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDictModel[]>?> ResponseHandleActionAsync(FormFieldModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.FindSessionsDocumentsByFormFieldNameAsync(payload, token);
    }
}
