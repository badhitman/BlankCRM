﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// IssueHelpdeskModelDB
/// </summary>
[Index(nameof(AuthorIdentityUserId), nameof(ExecutorIdentityUserId), nameof(LastUpdateAt), nameof(RubricIssueId), nameof(StatusDocument)), Index(nameof(NormalizedNameUpper))]
public class IssueHelpdeskModelDB : IssueHelpdeskModel
{
    /// <inheritdoc/>
    public string? NormalizedNameUpper { get; set; }

    /// <summary>
    /// Rubric Issue
    /// </summary>
    public required int? RubricIssueId { get; set; }

    /// <summary>
    /// Messages
    /// </summary>
    public List<IssueMessageHelpdeskModelDB>? Messages { get; set; }

    /// <summary>
    /// ReadMarkers
    /// </summary>
    public List<IssueReadMarkerHelpdeskModelDB>? ReadMarkers { get; set; }

    /// <summary>
    /// События
    /// </summary>
    public List<PulseIssueModelDB>? PulseEvents { get; set; }
}