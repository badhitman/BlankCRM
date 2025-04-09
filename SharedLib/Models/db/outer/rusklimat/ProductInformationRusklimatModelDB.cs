////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ProductInformationRusklimatModelDB
/// </summary>
public class ProductInformationRusklimatModelDB : EntryModel
{
    /// <summary>
    /// Product
    /// </summary>
    public ProductRusklimatModelDB? Product { get; set; }
    /// <summary>
    /// Product
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Тип информации (имя поля во входящем JSON объекте)
    /// </summary>
    /// <remarks>
    /// Pictures, Certificates, Video, RelatedProducts e.t.c
    /// </remarks>
    public required string TypeInfo { get; set; }
}