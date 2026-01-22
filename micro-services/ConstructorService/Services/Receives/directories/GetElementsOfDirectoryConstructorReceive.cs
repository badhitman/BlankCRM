////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Получить элементы справочника/списка
/// </summary>
public class GetElementsOfDirectoryConstructorReceive(IConstructorService conService) 
    : IResponseReceive<int, TResponseModel<List<EntryStandardModel>?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetElementsOfDirectoryConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<EntryStandardModel>?>?> ResponseHandleActionAsync(int payload, CancellationToken token = default)
    {
        return await conService.GetElementsOfDirectoryAsync(payload, token);
    }
}