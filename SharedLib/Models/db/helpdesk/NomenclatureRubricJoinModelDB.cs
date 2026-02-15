////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// NomenclatureRubricJoinModelDB
/// </summary>
[Index(nameof(RubricId))]
public class NomenclatureRubricJoinModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Rubric [FK]
    /// </summary>
    public int RubricId { get; set; }

    /// <summary>
    /// Nomenclature
    /// </summary>
    public NomenclatureModelDB? Nomenclature { get; set; }
    /// <summary>
    /// Nomenclature [FK]
    /// </summary>
    public int NomenclatureId { get; set; }
}