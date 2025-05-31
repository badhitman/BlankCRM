﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Номенклатура
/// </summary>
[Index(nameof(NormalizedNameUpper)), Index(nameof(ContextName))]
[Index(nameof(SortIndex), nameof(ParentId), nameof(ContextName), IsUnique = true)]
[Index(nameof(Name)), Index(nameof(IsDisabled))]
public class NomenclatureModelDB : UniversalLayerModel
{
    /// <summary>
    /// Базовая единица измерения `Номенклатуры`
    /// </summary>
    public UnitsOfMeasurementEnum BaseUnit { get; set; }

    /// <summary>
    /// Торговые предложения по Номенклатуре
    /// </summary>
    public List<OfferModelDB>? Offers { get; set; }

    /// <inheritdoc/>
    public static bool operator ==(NomenclatureModelDB off1, NomenclatureModelDB off2) => off1.Equals(off2);

    /// <inheritdoc/>
    public static bool operator !=(NomenclatureModelDB off1, NomenclatureModelDB off2)
    {
        return
                off1.Id != off2.Id ||
                off1.Name != off2.Name ||
                off1.BaseUnit != off2.BaseUnit ||
                off1.ParentId != off2.ParentId ||
                off1.ProjectId != off2.ProjectId ||
                off1.SortIndex != off2.SortIndex ||
                off1.IsDisabled != off2.IsDisabled ||
                off1.Description != off2.Description ||
                off1.ContextName != off2.ContextName ||
                off1.CreatedAtUTC != off2.CreatedAtUTC ||
                off1.NormalizedNameUpper != off2.NormalizedNameUpper;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is NomenclatureModelDB off)
            return
                off.Id == Id &&
                off.Name == Name &&
                off.BaseUnit == BaseUnit &&
                off.ParentId == ParentId &&
                off.ProjectId == ProjectId &&
                off.SortIndex == SortIndex &&
                off.IsDisabled == IsDisabled &&
                off.Description == Description &&
                off.ContextName == ContextName &&
                off.CreatedAtUTC == CreatedAtUTC &&
                off.NormalizedNameUpper == NormalizedNameUpper;

        return base.Equals(obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return $"{IsDisabled}|{Id}|{Name}|{BaseUnit}|{ParentId}|{ProjectId}|{SortIndex}|{ContextName}|{CreatedAtUTC}|{NormalizedNameUpper}|{Description}".GetHashCode();
    }
}