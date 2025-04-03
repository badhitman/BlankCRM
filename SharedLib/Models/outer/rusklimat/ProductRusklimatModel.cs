////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// ProductRusklimatModel
/// </summary>
public class ProductRusklimatModel : EntryAltModel
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
    public JObject? Properties { get; set; }

    /// <inheritdoc/>
    public List<string>? Pictures { get; set; }

    /// <inheritdoc/>
    public string? Description { get; set; }

    /// <inheritdoc/>
    public List<string>? Certificates { get; set; }

    /// <summary>
    /// индивидуальная цена партнёра, в случае, когда цена не установлена, будет отдан 0
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// цена РИЦ
    /// </summary>
    public double? InternetPrice { get; set; }

    /// <inheritdoc/>
    public int ClientPrice { get; set; }

    /// <summary>
    /// признак эксклюзивности
    /// </summary>
    public bool Exclusive { get; set; }
    
    /// <inheritdoc/>
    public List<string>? Video { get; set; }

    /// <summary>
    /// массив уникальных идентификаторов сопутствующих товаров, может содержать 0 элементов
    /// </summary>
    public List<string>? RelatedProducts { get; set; }

    /// <summary>
    /// массив уникальных идентификаторов аналогичных товаров, может содержать 0 элементов
    /// </summary>
    public List<string>? Analog { get; set; }
    /// <summary>
    /// массив ссылок на pdf файлы с чертежами, может содержать 0 элементов
    /// </summary>
    public List<string>? Drawing { get; set; }

    /// <summary>
    /// массив ссылок на pdf файлы с промо материалами, может содержать 0 элементов
    /// </summary>
    public List<string>? PromoMaterials { get; set; }
    
    /// <summary>
    /// массив ссылок на pdf файлы с инструкциями, может содержать 0 элементов
    /// </summary>
    public List<string>? Instructions { get; set; }

    /// <summary>
    /// массив штрихкодов
    /// </summary>
    public List<string>? Barcode { get; set; }

    /// <summary>
    /// остаток товара на складе
    /// </summary>
    public RemainsRusklimatModel? Remains { get; set; }
}