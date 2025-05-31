////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Рубрики для обращений
/// </summary>
[Index(nameof(NormalizedNameUpper)), Index(nameof(ContextName)), Index(nameof(Name)), Index(nameof(IsDisabled))]
[Index(nameof(SortIndex), nameof(ParentId), nameof(ContextName), IsUnique = true)]
[Index(nameof(NormalizedNameUpper)), Index(nameof(ContextName))]
public class RubricIssueHelpDeskModelDB : UniversalLayerModel
{
    /// <inheritdoc/>
    public List<RubricIssueHelpDeskModelDB>? NestedRubrics { get; set; }

    /// <summary>
    /// Обращения в рубрике
    /// </summary>
    public List<IssueHelpDeskModelDB>? Issues { get; set; }

    /// <summary>
    /// ArticlesJoins
    /// </summary>
    public List<RubricArticleJoinModelDB>? ArticlesJoins { get; set; }

    /// <summary>
    /// Владелец (вышестоящая рубрика)
    /// </summary>
    public RubricIssueHelpDeskModelDB? Parent { get; set; }


    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj is RubricIssueHelpDeskModelDB e)
            return Name == e.Name && Description == e.Description && Id == e.Id && e.SortIndex == SortIndex && e.ParentId == ParentId && e.ProjectId == ProjectId;

        return false;
    }

    /// <inheritdoc/>
    public static bool operator ==(RubricIssueHelpDeskModelDB? e1, RubricIssueHelpDeskModelDB? e2)
        =>
        (e1 is null && e2 is null) ||
        (e1?.Id == e2?.Id && e1?.Name == e2?.Name && e1?.Description == e2?.Description && e1?.SortIndex == e2?.SortIndex && e1?.ParentId == e2?.ParentId && e1?.ProjectId == e2?.ProjectId);

    /// <inheritdoc/>
    public static bool operator !=(RubricIssueHelpDeskModelDB? e1, RubricIssueHelpDeskModelDB? e2)
        =>
        (e1 is null && e2 is not null) ||
        (e1 is not null && e2 is null) ||
        e1?.Id != e2?.Id ||
        e1?.Name != e2?.Name ||
        e1?.Description != e2?.Description ||
        e1?.SortIndex != e2?.SortIndex ||
        e1?.ParentId != e2?.ParentId ||
        e1?.ProjectId != e2?.ProjectId;

    /// <inheritdoc/>
    public override int GetHashCode()
    => $"{ParentId} {SortIndex} {Name} {Id} {Description}".GetHashCode();
}