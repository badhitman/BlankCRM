////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Рубрики
/// </summary>
[Index(nameof(NormalizedNameUpper)), Index(nameof(ContextName)), Index(nameof(Name)), Index(nameof(IsDisabled))]
[Index(nameof(SortIndex), nameof(ParentId), nameof(ContextName), IsUnique = true), Index(nameof(PrefixName))]
public class RubricModelDB : RubricStandardModel
{
    /// <inheritdoc/>
    public new List<RubricModelDB>? NestedRubrics { get; set; }

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
    public new RubricModelDB? Parent { get; set; }

    /// <inheritdoc/>
    public static RubricModelDB Build(RubricStandardModel sender)
    {
        return new()
        {
            ContextName = sender.ContextName,
            CreatedAtUTC = sender.CreatedAtUTC,
            Description = sender.Description,
            Id = sender.Id,
            IsDisabled = sender.IsDisabled,
            LastUpdatedAtUTC = sender.LastUpdatedAtUTC,
            ParentId = sender.ParentId,
            ProjectId = sender.ProjectId,
            Name = sender.Name,
            NormalizedNameUpper = sender.NormalizedNameUpper,
            SortIndex = sender.SortIndex,

            NestedRubrics = sender.NestedRubrics is null ? null : [.. sender.NestedRubrics.Select(Build)],
            Parent = sender.Parent is null ? null : Build(sender.Parent),
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj is RubricModelDB e)
            return Name == e.Name && Description == e.Description && Id == e.Id && e.SortIndex == SortIndex && e.ParentId == ParentId && e.ProjectId == ProjectId;

        return false;
    }

    /// <inheritdoc/>
    public static bool operator ==(RubricModelDB? e1, RubricModelDB? e2)
        =>
        (e1 is null && e2 is null) ||
        (e1?.Id == e2?.Id && e1?.Name == e2?.Name && e1?.Description == e2?.Description && e1?.SortIndex == e2?.SortIndex && e1?.ParentId == e2?.ParentId && e1?.ProjectId == e2?.ProjectId);

    /// <inheritdoc/>
    public static bool operator !=(RubricModelDB? e1, RubricModelDB? e2)
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