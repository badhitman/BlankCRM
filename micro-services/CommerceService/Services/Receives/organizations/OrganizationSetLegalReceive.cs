////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Organization set legal
/// </summary>
public class OrganizationSetLegalReceive(ICommerceService commerceRepo, ILogger<OrganizationSetLegalReceive> loggerRepo) : IResponseReceive<OrganizationLegalModel?, TResponseModel<bool>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationSetLegalCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>?> ResponseHandleActionAsync(OrganizationLegalModel? org, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(org);
        loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(org)}");
        return await commerceRepo.OrganizationSetLegalAsync(org, token);
    }
}