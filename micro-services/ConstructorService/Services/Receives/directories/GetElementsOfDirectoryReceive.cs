////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Получить элементы справочника/списка
/// </summary>
public class GetElementsOfDirectoryReceive(IConstructorService conService) : IResponseReceive<int, TResponseModel<List<EntryModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetElementsOfDirectoryReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<EntryModel>>?> ResponseHandleActionAsync(int payload, CancellationToken token = default)
    {
        return await conService.GetElementsOfDirectoryAsync(payload, token);
    }
}