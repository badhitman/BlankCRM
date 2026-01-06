////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Подобрать формы
/// </summary>
public class SelectFormsConstructorReceive(IConstructorService conService) 
    : IResponseReceive<SelectFormsModel?, TPaginationResponseStandardModel<FormConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectFormsConstructorReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<FormConstructorModelDB>?> ResponseHandleActionAsync(SelectFormsModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.SelectFormsAsync(payload, token);
    }
}