////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Universal update - request
/// </summary>
public class UniversalUpdateRequestModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Имя объекта
    /// </summary>
    [NameValid, Required]
    public virtual string Name { get; set; }

    /// <summary>
    /// Описание/примечание для объекта
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Parent ID
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// ProjectId
    /// </summary>
    public int ProjectId { get; set; }
}