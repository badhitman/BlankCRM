////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Найти порцию сессий по имени поля (с пагинацией)
/// </summary>
public class FindSessionsDocumentsByFormFieldNameConstructorReceive(IConstructorService conService) 
    : IResponseReceive<FormFieldModel?, TResponseModel<EntryDictStandardModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FindSessionsDocumentsByFormFieldNameConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDictStandardModel[]>?> ResponseHandleActionAsync(FormFieldModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.FindSessionsDocumentsByFormFieldNameAsync(payload, token);
    }
}
