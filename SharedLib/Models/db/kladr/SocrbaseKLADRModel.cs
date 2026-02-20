////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
public class SocrbaseKLADRModelDTO : SocrbaseKLADRModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public static SocrbaseTempKLADRModelDB Build(SocrbaseKLADRModel x)
    {
        return new()
        {
            KOD_T_ST = x.KOD_T_ST,
            LEVEL = x.LEVEL,
            SCNAME = x.SCNAME,
            SOCRNAME = x.SOCRNAME,
        };
    }
}

/// <inheritdoc/>
[Index(nameof(SCNAME)), Index(nameof(SOCRNAME)), Index(nameof(KOD_T_ST)), Index(nameof(LEVEL))]
public class SocrbaseKLADRModelDB : SocrbaseKLADRModelDTO
{

}

/// <inheritdoc/>
public class SocrbaseTempKLADRModelDB : SocrbaseKLADRModelDTO
{

}