﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// ConsoleIssuesSelectReceive
/// </summary>
public class ConsoleIssuesSelectReceive(IHelpdeskService hdRepo) : IResponseReceive<TPaginationRequestModel<ConsoleIssuesRequestModel>?, TPaginationResponseModel<IssueHelpdeskModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.ConsoleIssuesSelectHelpdeskReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<IssueHelpdeskModel>?> ResponseHandleActionAsync(TPaginationRequestModel<ConsoleIssuesRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.ConsoleIssuesSelectAsync(req, token);
    }
}