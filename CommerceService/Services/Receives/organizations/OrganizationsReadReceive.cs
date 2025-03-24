////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OrganizationsReadReceive
/// </summary>
public class OrganizationsReadReceive(ICommerceService commerceRepo) : IResponseReceive<int[]?, TResponseModel<OrganizationModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.OrganizationsReadCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<OrganizationModelDB[]>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.OrganizationsRead(req, token);
    }
}