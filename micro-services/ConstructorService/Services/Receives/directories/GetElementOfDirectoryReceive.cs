////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Получить элемент справочника/перечисления/списка
/// </summary>
public class GetElementOfDirectoryReceive(IConstructorService conService) 
    : IResponseReceive<int, TResponseModel<EntryDescriptionModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetElementOfDirectoryReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDescriptionModel>?> ResponseHandleActionAsync(int payload, CancellationToken token = default)
    {
        return await conService.GetElementOfDirectoryAsync(payload, token);
    }
}