﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// LockTransactionModelDB
/// </summary>
[Index(nameof(LockerId), nameof(LockerName), nameof(RubricId), IsUnique = true)]
public class LockTransactionModelDB
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
    public required int LockerId { get; set; }

    /// <summary>
    /// Склад
    /// </summary>
    public required int RubricId { get; set; }
}