////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// ParamsProductDaichiModelDB
/// </summary>
public class ParamsProductDaichiModelDB: ParamsProductDaichiModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public ProductDaichiModelDB? Product {  get; set; }
    /// <inheritdoc/>
    public int ProductId { get; set; }
}