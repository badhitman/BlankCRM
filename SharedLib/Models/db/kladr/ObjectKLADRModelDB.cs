////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
[Index(nameof(STATUS))]
public class ObjectKLADRModelDB : RootKLADRModelDB
{
    /// <inheritdoc/>
    [Required, StringLength(1)]
    public required string STATUS { get; set; }

    /// <inheritdoc/>
    public static ObjectKLADRModelDB Build(ObjectKLADRModel x)
    {
        return new()
        {
            CODE = x.CODE,
            GNINMB = x.GNINMB,
            INDEX = x.INDEX,
            NAME = x.NAME,
            OCATD = x.OCATD,
            SOCR = x.SOCR,
            STATUS = x.STATUS,
            UNO = x.UNO,
        };
    }
}