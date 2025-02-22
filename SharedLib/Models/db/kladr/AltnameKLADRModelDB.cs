////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <inheritdoc/>
[Index(nameof(OLDCODE)), Index(nameof(NEWCODE)), Index(nameof(LEVEL))]
public class AltnameKLADRModelDB : AltnameKLADRModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public static AltnameKLADRModelDB Build(AltnameKLADRModel x)
    {
        return new()
        {
            LEVEL = x.LEVEL,
            NEWCODE = x.NEWCODE,
            OLDCODE = x.OLDCODE,
        };
    }
}