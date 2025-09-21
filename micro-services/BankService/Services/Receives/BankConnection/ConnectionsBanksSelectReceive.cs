﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// ConnectionsBanksSelectReceive
/// </summary>
public class ConnectionsBanksSelectReceive(IBankService bankRepo) 
    : IResponseReceive<TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel>?, TPaginationResponseModel<BankConnectionModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionsBanksSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<BankConnectionModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await bankRepo.ConnectionsBanksSelectAsync(req, token);
    }
}