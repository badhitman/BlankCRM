﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
[Index(nameof(SHNAME)), Index(nameof(SCNAME))]
public class NameMapKLADRModelDB : BaseKladrModel
{
    /// <inheritdoc/>
    [Required, StringLength(40)]
    public required string SHNAME { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(10)]
    public required string SCNAME { get; set; }

    /// <inheritdoc/>
    public static NameMapKLADRModelDB Build(NameMapKLADRModel x)
    {
        return new()
        {
            CODE = x.CODE,
            NAME = x.NAME,
            SCNAME = x.SCNAME,
            SHNAME = x.SHNAME,
        };
    }
}