////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// Рубрики
/// </summary>
public class RubricStandardModel : UniversalLayerModel
{
    /// <summary>
    /// Имя-префикс
    /// </summary>
    /// <remarks>
    /// Для организации внутри одного контекста разных наборов рубрик
    /// </remarks>
    public string? PrefixName { get; set; }

    /// <inheritdoc/>
    public virtual List<RubricStandardModel>? NestedRubrics { get; set; }

    /// <summary>
    /// Владелец (вышестоящая рубрика)
    /// </summary>
    public virtual RubricStandardModel? Parent { get; set; }


    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        if (obj is RubricStandardModel e)
            return Name == e.Name && Description == e.Description && Id == e.Id && e.SortIndex == SortIndex && e.ParentId == ParentId && e.ProjectId == ProjectId;

        return false;
    }

    /// <inheritdoc/>
    public static bool operator ==(RubricStandardModel e1, RubricStandardModel e2)
        =>
        (e1 is null && e2 is null) ||
        (e1?.Id == e2?.Id && e1?.Name == e2?.Name && e1?.Description == e2?.Description && e1?.SortIndex == e2?.SortIndex && e1?.ParentId == e2?.ParentId && e1?.ProjectId == e2?.ProjectId);

    /// <inheritdoc/>
    public static bool operator !=(RubricStandardModel e1, RubricStandardModel e2)
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