﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UsersOrganizationsSelectReceive
/// </summary>
public class UsersOrganizationsSelectReceive(ICommerceService commerceRepo) : IResponseReceive<TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel>?, TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationsUsersSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<UserOrganizationModelDB>>?> ResponseHandleActionAsync(TPaginationRequestAuthModel<UsersOrganizationsStatusesRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.UsersOrganizationsSelectAsync(req, token);
    }
}