////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// ProductParamsRequestDaichiModel
/// </summary>
public class ProductParamsRequestDaichiModel: RequestDaichiBaseModel
{
    /// <summary>
    /// Количество товаров на странице. Максимально значение 100.
    /// </summary>
    /// <remarks>
    /// Требуется указать, если не были указаны параметры filter[NAME] или filter[XML_ID].
    /// Без указания данного параметра метод вернёт список всех товаров из каталога, но без характеристик.
    /// </remarks>
    [JsonProperty("page-size"),JsonPropertyName("page-size")]
    public int PageSize { get; set; }

    /// <summary>
    /// Номер страницы.
    /// </summary>
    [JsonProperty("page"), JsonPropertyName("page")]
    public int Page {  get; set; }

    /// <summary>
    /// Флаг для показа незаполненных характеристик товара (по-умолчанию false). 
    /// </summary>
    [JsonProperty("show-empty-params"), JsonPropertyName("show-empty-params")]
    public bool ShowEmptyParams { get; set; }
}