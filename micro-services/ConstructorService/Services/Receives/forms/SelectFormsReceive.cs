////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Подобрать формы
/// </summary>
public class SelectFormsReceive(IConstructorService conService) 
    : IResponseReceive<SelectFormsModel?, TPaginationResponseModel<FormConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectFormsReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<FormConstructorModelDB>?> ResponseHandleActionAsync(SelectFormsModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.SelectFormsAsync(payload, token);
    }
}