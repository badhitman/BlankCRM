////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// ProductRusklimatModel
/// </summary>
public class ProductRusklimatModel : ProductRusklimatBaseModel
{
    /// <inheritdoc/>
    public List<string>? Pictures { get; set; }

    /// <inheritdoc/>
    public List<string>? Certificates { get; set; }

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


    /// <summary>
    /// Массив свойств товара.
    /// </summary>
    /// <remarks>
    /// элементы в формате:
    /// для версии метода v1 - "уникальный идентификатор свойства": "значение свойства"
    /// для версии метода v2 - "уникальный идентификатор свойства": {"value": "значение свойства", "unit": "идентификатор единицы измерения(информацию по единицам измерения можно получить в другом методе - /api/v1/InternetPartner/units, см. Получение единиц измерения)"}
    /// </remarks>
    public Dictionary<string, JObject>? Properties { get; set; }
}