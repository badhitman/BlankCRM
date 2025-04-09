////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// ProductBreezRuBaseModel
/// </summary>
[Index(nameof(NC)), Index(nameof(VnutrNC)), Index(nameof(Title)), Index(nameof(Series))]
[Index(nameof(CategoryId)), Index(nameof(Article)), Index(nameof(Brand)), Index(nameof(UTP))]
public class ProductBreezRuBaseModel : ProductBreezRuLiteModel
{
    /// <summary>
    /// Артикул
    /// </summary>
    [JsonProperty("articul"), JsonPropertyName("articul")]
    public string? Article { get; set; }

    /// <summary>
    /// Идентификатор категории
    /// </summary>
    [JsonProperty("category_id"), JsonPropertyName("category_id")]
    public string? CategoryId { get; set; }

    /// <summary>
    /// Название серии
    /// </summary>
    public string? Series { get; set; }

    /// <summary>
    /// Название продукта
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Идентификатор бренда
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// УТП
    /// </summary>
    public string? UTP { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// URL файла с буклетом
    /// </summary>
    public string? Booklet { get; set; }

    /// <summary>
    /// URL файла с инструкцией
    /// </summary>
    public string? Manual { get; set; }

    /// <summary>
    /// URL файла с моделью
    /// </summary>
    [JsonProperty("bim_model"), JsonPropertyName("bim_model")]
    public string? BimModel { get; set; }

    /// <summary>
    /// Идентификатор видео на YouTube
    /// </summary>
    [JsonProperty("video_youtube"), JsonPropertyName("video_youtube")]
    public string? VideoYoutube { get; set; }
}