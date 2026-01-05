////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Сдвинуть связь [таба/вкладки схемы документа] с [формой] (изменение сортировки/последовательности)
/// </summary>
public class MoveTabDocumentSchemeJoinFormReceive(IConstructorService conService) 
    : IResponseReceive<TAuthRequestModel<MoveObjectModel>?, TResponseModel<TabOfDocumentSchemeConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MoveTabDocumentSchemeJoinFormReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>?> ResponseHandleActionAsync(TAuthRequestModel<MoveObjectModel>? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.MoveTabDocumentSchemeJoinFormAsync(payload, token);
    }
}
