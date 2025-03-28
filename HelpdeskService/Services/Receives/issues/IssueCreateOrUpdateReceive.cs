﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Create (or update) Issue: Рубрика, тема и описание
/// </summary>
public class IssueCreateOrUpdateReceive(IHelpdeskService hdRepo) : IResponseReceive<TAuthRequestModel<UniversalUpdateRequestModel>?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.IssueUpdateHelpdeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TAuthRequestModel<UniversalUpdateRequestModel>? issue_upd, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(issue_upd);
        return await hdRepo.IssueCreateOrUpdateAsync(issue_upd, token);
    }
}