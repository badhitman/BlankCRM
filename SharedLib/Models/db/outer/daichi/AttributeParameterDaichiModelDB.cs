////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// AttributeParameterDaichiModelDB
/// </summary>
public class AttributeParameterDaichiModelDB : AttributeParameterDaichiModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }


    /// <inheritdoc/>
    public ParameterEntryDaichiModelDB? Parent { get; set; }
    /// <inheritdoc/>
    public int ParentId { get; set; }

    /// <inheritdoc/>
    public static AttributeParameterDaichiModelDB Build(AttributeParameterDaichiModel y, ParameterEntryDaichiModelDB res)
    {
        return new AttributeParameterDaichiModelDB()
        {
            CODE = y.CODE,
            NAME = y.NAME,
            VALUE = y.VALUE,
            GROUP = y.GROUP,
            Parent = res
        };
    }
}