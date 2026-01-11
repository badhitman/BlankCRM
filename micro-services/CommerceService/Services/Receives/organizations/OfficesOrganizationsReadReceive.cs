////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OfficesOrganizationsReadReceive
/// </summary>
public class OfficesOrganizationsReadReceive(ICommerceService commerceRepo)
    : IResponseReceive<int[]?, TResponseModel<OfficeOrganizationModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OfficesOrganizationsReadCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<OfficeOrganizationModelDB[]>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.OfficesOrganizationsReadAsync(req, token);
    }
}