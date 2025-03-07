////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
public class ObjectKLADRModelDTO : RootKLADRModelDB
{
    /// <inheritdoc/>
    [Required, StringLength(1)]
    public required string STATUS { get; set; }

    /// <inheritdoc/>
    public static ObjectTempKLADRModelDB Build(ObjectKLADRModel x)
    {
        return new()
        {
            STATUS = x.STATUS,

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
[Index(nameof(STATUS))]
[Index(nameof(SOCR)), Index(nameof(INDEX)), Index(nameof(GNINMB)), Index(nameof(UNO)), Index(nameof(OCATD))]
public class ObjectKLADRModelDB : ObjectKLADRModelDTO
{

}

/// <inheritdoc/>
public class ObjectTempKLADRModelDB : ObjectKLADRModelDTO
{

}