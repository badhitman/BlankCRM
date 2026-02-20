////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Простой запрос (с пагинацией)  SimplePaginationRequestModel
/// </summary>
public class SimplePaginationRequestStandardModel : PaginationRequestStandardModel
{
    /// <summary>
    /// Строка запроса
    /// </summary>
    public string? FindQuery { get; set; }

    /// <inheritdoc/>
    public static SimplePaginationRequestStandardModel Build(string? searchString, int pageSize, int page)
    {
        return new()
        {
            FindQuery = searchString,
            PageSize = pageSize,
            PageNum = page,
        };
    }
}