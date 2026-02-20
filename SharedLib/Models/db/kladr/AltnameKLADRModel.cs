////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
public class AltnameKLADRModelDTO : AltnameKLADRModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public static AltnameTempKLADRModelDB Build(AltnameKLADRModel x)
    {
        return new()
        {
            LEVEL = x.LEVEL,
            NEWCODE = x.NEWCODE,
            OLDCODE = x.OLDCODE,
        };
    }
}

/// <inheritdoc/>
[Index(nameof(OLDCODE)), Index(nameof(NEWCODE)), Index(nameof(LEVEL))]
public class AltnameKLADRModelDB : AltnameKLADRModelDTO
{
    
}

/// <inheritdoc/>
public class AltnameTempKLADRModelDB : AltnameKLADRModelDTO
{
    
}