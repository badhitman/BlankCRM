////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Получить связь [таба/вкладки схемы документа] с [формой]
/// </summary>
public class GetTabDocumentSchemeJoinFormReceive(IConstructorService conService) 
    : IResponseReceive<int, TResponseModel<FormToTabJoinConstructorModelDB?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetTabDocumentSchemeJoinFormConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<FormToTabJoinConstructorModelDB?>?> ResponseHandleActionAsync(int payload, CancellationToken token = default)
    {
        return await conService.GetTabDocumentSchemeJoinFormAsync(payload, token);
    }
}
