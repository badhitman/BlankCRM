////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OfficesOrganizationsReadReceive
/// </summary>
public class OfficesOrganizationsReadReceive(ICommerceService commerceRepo) : IResponseReceive<int[]?, TResponseModel<OfficeOrganizationModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.OfficesOrganizationsReadCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<OfficeOrganizationModelDB[]>?> ResponseHandleAction(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.OfficesOrganizationsRead(req, token);
    }
}