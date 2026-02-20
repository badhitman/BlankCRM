////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// ApiRestBaseModel
/// </summary>
public abstract class ApiRestBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Имя объекта
    /// </summary>
    [Required(AllowEmptyStrings = true)]
    public virtual required string Name { get; set; }

    /// <summary>
    /// Родитель
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// Родитель
    /// </summary>
    public ApiRestConfigModelDB? Parent { get; set; }
}