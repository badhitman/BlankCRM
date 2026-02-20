////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
public class NameMapKLADRModelDTO : BaseKladrModel
{
    /// <inheritdoc/>
    [Required, StringLength(17)]
    public override required string CODE { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(40)]
    public required string SHNAME { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(10)]
    public required string SCNAME { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(250)]
    public override required string NAME { get; set; }

    /// <inheritdoc/>
    public static NameMapTempKLADRModelDB Build(NameMapKLADRModel x)
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

/// <inheritdoc/>
[Index(nameof(SHNAME)), Index(nameof(SCNAME))]
[Index(nameof(NAME)), Index(nameof(CODE))]
public class NameMapKLADRModelDB : NameMapKLADRModelDTO
{

}

/// <inheritdoc/>
public class NameMapTempKLADRModelDB : NameMapKLADRModelDTO
{

}