////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// ProductRusklimatBaseModel
/// </summary>
public class ProductRusklimatBaseModel : EntryAltStandardModel
{
    /// <summary>
    /// НС код товара
    /// </summary>
    public string? NSCode { get; set; }

    /// <summary>
    /// уникальный идентификатор категории, к которой привязан товар
    /// </summary>
    public string? CategoryId { get; set; }

    /// <summary>
    /// артикул товара
    /// </summary>
    public string? VendorCode { get; set; }

    /// <inheritdoc/>
    public string? Brand { get; set; }

    /// <inheritdoc/>
    public string? Description { get; set; }

    /// <summary>
    /// индивидуальная цена партнёра, в случае, когда цена не установлена, будет отдан 0
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// индивидуальная цена партнёра, в случае, когда цена не установлена, будет отдан 0
    /// </summary>
    public decimal ClientPrice { get; set; }

    /// <summary>
    /// цена РИЦ
    /// </summary>
    public decimal? InternetPrice { get; set; }

    /// <summary>
    /// признак эксклюзивности
    /// </summary>
    public bool Exclusive { get; set; }
}