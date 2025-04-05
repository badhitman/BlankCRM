////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ProductRusklimatBaseModel
/// </summary>
public class ProductRusklimatBaseModel : EntryAltModel
{
    /// <inheritdoc/>
    public string? NSCode { get; set; }

    /// <inheritdoc/>
    public string? CategoryId { get; set; }

    /// <inheritdoc/>
    public string? VendorCode { get; set; }

    /// <inheritdoc/>
    public string? Brand { get; set; }

    /// <inheritdoc/>
    public string? Description { get; set; }

    /// <summary>
    /// индивидуальная цена партнёра, в случае, когда цена не установлена, будет отдан 0
    /// </summary>
    public decimal Price { get; set; }

    /// <inheritdoc/>
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