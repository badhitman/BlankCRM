////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// Запрос (с пагинацией)
/// </summary>
public class RusklimatPaginationRequestModel : PaginationRequestModel
{
    /// <summary>
    /// массив полей товаров, которые необходимо выводить в ответе, если не указан, будут выведены все поля
    /// </summary>
    public List<string>? Columns { get; set; }

    /// <summary>
    /// Filter
    /// </summary>
    public FilterRusklimatRequestModel? Filter { get; set; }

    /// <inheritdoc/>
    [JsonProperty("sort"), JsonPropertyName("sort")]
    public Dictionary<string, string> Sort { get; set; } = new Dictionary<string, string>() { { "nsCode", "asc" } };

    /// <inheritdoc/>
    [JsonProperty("extraStuff"), JsonPropertyName("extraStuff")]
    public Dictionary<string, string[]>? ExtraStuff { get; set; }

    /// <inheritdoc/>
    public static RusklimatPaginationRequestModel Build(int pageSize, int page)
    {
        return new()
        {
            PageSize = pageSize,
            PageNum = page,
        };
    }
}