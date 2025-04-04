﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Получить рубрики
/// </summary>
public class RubricsGetReceive(IHelpdeskService hdRepo) : IResponseReceive<int[]?, TResponseModel<List<RubricIssueHelpdeskModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.RubricsForIssuesGetHelpdeskReceive;

    /// <summary>
    /// Получить рубрики
    /// </summary>
    public async Task<TResponseModel<List<RubricIssueHelpdeskModelDB>>?> ResponseHandleActionAsync(int[]? rubricsIds, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(rubricsIds);
        return await hdRepo.RubricsGetAsync(rubricsIds, token);
    }
}