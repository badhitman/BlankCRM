﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// NomenclaturesSelectReceive
/// </summary>
public class NomenclaturesSelectReceive(ICommerceService commerceRepo) : IResponseReceive<TPaginationRequestModel<NomenclaturesSelectRequestModel>?, TPaginationResponseModel<NomenclatureModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.NomenclaturesSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NomenclatureModelDB>?> ResponseHandleActionAsync(TPaginationRequestModel<NomenclaturesSelectRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.NomenclaturesSelectAsync(req, token);
    }
}