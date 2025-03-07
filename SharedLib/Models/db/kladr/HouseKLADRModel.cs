////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Дом
/// </summary>
public class HouseKLADRModelDTO : RootKLADRModelDB
{
    /// <inheritdoc/>
    [Required, StringLength(19)]
    public override required string CODE { get; set; }

    /// <inheritdoc/>
    [Required, StringLength(10)]
    public required string KORP { get; set; }

    /// <inheritdoc/>
    public static HouseTempKLADRModelDB Build(HouseKLADRModel x)
    {
        return new()
        {
            CODE = x.CODE,
            GNINMB = x.GNINMB,
            INDEX = x.INDEX,
            KORP = x.KORP,
            NAME = x.NAME,
            OCATD = x.OCATD,
            SOCR = x.SOCR,
            UNO = x.UNO,
        };
    }
}

/// <summary>
/// Дом
/// </summary>
[Index(nameof(SOCR)), Index(nameof(INDEX)), Index(nameof(GNINMB)), Index(nameof(UNO)), Index(nameof(OCATD))]
public class HouseKLADRModelDB : HouseKLADRModelDTO
{

}

/// <summary>
/// Дом
/// </summary>
public class HouseTempKLADRModelDB : HouseKLADRModelDTO
{

}