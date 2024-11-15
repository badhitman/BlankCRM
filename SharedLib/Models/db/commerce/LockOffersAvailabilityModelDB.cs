﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// LockOffersAvailabilityModelDB
/// </summary>
[Index(nameof(LockerId), nameof(LockerName), IsUnique = true)]
public class LockOffersAvailabilityModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// LockerName
    /// </summary>
    public required string LockerName { get; set; }

    /// <summary>
    /// LockerId
    /// </summary>
    public int LockerId { get; set; }

    /// <summary>
    /// Rubric
    /// </summary>
    public int? RubricId { get; set; }
}