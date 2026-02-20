////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Сдвинуть связь [таба/вкладки схемы документа] с [формой] (изменение сортировки/последовательности)
/// </summary>
public class MoveTabDocumentSchemeJoinFormConstructorReceive(IConstructorService conService) 
    : IResponseReceive<TAuthRequestStandardModel<MoveObjectModel>?, TResponseModel<TabOfDocumentSchemeConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MoveTabDocumentSchemeJoinFormConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<MoveObjectModel>? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.MoveTabDocumentSchemeJoinFormAsync(payload, token);
    }
}
