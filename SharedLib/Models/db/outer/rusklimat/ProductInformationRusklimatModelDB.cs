////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// ProductInformationRusklimatModelDB
/// </summary>
[Index(nameof(TypeInfo))]
public class ProductInformationRusklimatModelDB : EntryModel
{
    /// <summary>
    /// Product
    /// </summary>
    public ProductRusklimatModelDB? Product { get; set; }
    /// <summary>
    /// Product
    /// </summary>
    public string ProductId { get; set; } = default!;

    /// <summary>
    /// Тип информации (имя поля во входящем JSON объекте)
    /// </summary>
    /// <remarks>
    /// Pictures, Certificates, Video, RelatedProducts e.t.c
    /// </remarks>
    public required string TypeInfo { get; set; }
}