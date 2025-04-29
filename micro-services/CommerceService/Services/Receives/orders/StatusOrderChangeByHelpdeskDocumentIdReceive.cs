////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// StatusOrderChangeByHelpDeskDocumentIdReceive
/// </summary>
public class StatusOrderChangeByHelpDeskDocumentIdReceive(ICommerceService commRepo, ILogger<StatusOrderChangeByHelpDeskDocumentIdReceive> LoggerRepo) : IResponseReceive<TAuthRequestModel<StatusChangeRequestModel>?, TResponseModel<bool>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.StatusChangeOrderByHelpDeskDocumentIdReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>?> ResponseHandleActionAsync(TAuthRequestModel<StatusChangeRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await commRepo.StatusesOrdersChangeByHelpDeskDocumentIdAsync(req, token);
    }
}