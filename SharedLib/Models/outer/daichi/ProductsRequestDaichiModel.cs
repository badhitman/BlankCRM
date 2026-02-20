////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// ProductsRequestDaichiModel
/// </summary>
public class ProductsRequestDaichiModel : RequestDaichiBaseModel
{
    /// <summary>
    /// Количество товаров на странице. Максимально значение 100.
    /// </summary>
    /// <remarks>
    /// Требуется указать, если не были указаны параметры filter[NAME] или filter[XML_ID].
    /// Без указания данного параметра метод вернёт список всех товаров из каталога, но без характеристик.
    /// </remarks>
    [JsonProperty("store-id"), JsonPropertyName("store-id")]
    public string StoreId { get; set; } = "default";
}