////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
public class StreetKLADRModelDTO : RootKLADRModelDB
{
    /// <inheritdoc/>
    [Required, StringLength(17)]
    public override required string CODE { get; set; }

    /// <inheritdoc/>
    public static StreetTempKLADRModelDB Build(RootKLADRModel x)
    {
        return new()
        {
            CODE = x.CODE,
            GNINMB = x.GNINMB,
            INDEX = x.INDEX,
            NAME = x.NAME,
            OCATD = x.OCATD,
            SOCR = x.SOCR,
            UNO = x.UNO,
        };
    }
}

/// <inheritdoc/>
[Index(nameof(SOCR)), Index(nameof(INDEX)), Index(nameof(GNINMB)), Index(nameof(UNO)), Index(nameof(OCATD))]
public class StreetKLADRModelDB : StreetKLADRModelDTO
{

}

/// <inheritdoc/>
public class StreetTempKLADRModelDB : StreetKLADRModelDTO
{

}