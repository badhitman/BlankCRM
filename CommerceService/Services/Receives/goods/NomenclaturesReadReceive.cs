﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using RemoteCallLib;
using SharedLib;
using DbcLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// NomenclaturesReadReceive
/// </summary>
public class NomenclaturesReadReceive(ICommerceService commerceRepo)
    : IResponseReceive<TAuthRequestModel<int[]>?, TResponseModel<List<NomenclatureModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.NomenclaturesReadCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<NomenclatureModelDB>>?> ResponseHandleAction(TAuthRequestModel<int[]>? req)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.NomenclaturesRead(req);
    }
}