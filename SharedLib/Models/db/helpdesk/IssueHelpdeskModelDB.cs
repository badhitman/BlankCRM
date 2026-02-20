////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// IssueHelpDeskModelDB
/// </summary>
[Index(nameof(AuthorIdentityUserId), nameof(ExecutorIdentityUserId), nameof(LastUpdateAt), nameof(RubricIssueId), nameof(StatusDocument)), Index(nameof(NormalizedNameUpper))]
public class IssueHelpDeskModelDB : IssueHelpDeskModel
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
    public List<IssueMessageHelpDeskModelDB>? Messages { get; set; }

    /// <summary>
    /// ReadMarkers
    /// </summary>
    public List<IssueReadMarkerHelpDeskModelDB>? ReadMarkers { get; set; }

    /// <summary>
    /// События
    /// </summary>
    public List<PulseIssueModelDB>? PulseEvents { get; set; }
}