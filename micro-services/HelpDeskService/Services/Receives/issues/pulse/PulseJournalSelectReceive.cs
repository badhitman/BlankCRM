﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// PulseJournalReceive - of context user
/// </summary>
public class PulseJournalSelectReceive(IHelpDeskService hdRepo)
    : IResponseReceive<TAuthRequestModel<TPaginationRequestStandardModel<UserIssueModel>>?, TResponseModel<TPaginationResponseModel<PulseViewModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.PulseJournalHelpDeskSelectReceive;

    /// <summary>
    /// PulseJournalReceive - of context user
    /// </summary>
    public async Task<TResponseModel<TPaginationResponseModel<PulseViewModel>>?> ResponseHandleActionAsync(TAuthRequestModel<TPaginationRequestStandardModel<UserIssueModel>>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.PulseJournalSelectAsync(req, token);
    }
}