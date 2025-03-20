////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// KladrFindRequestModel
/// </summary>
public class KladrFindRequestModel : PaginationRequestModel
{
    /// <summary>
    /// Применение фильтра к коду (SQL Inject as LIKE operator)
    /// </summary>
    public string[]? CodeLikeFilter { get; set; }

    /// <summary>
    /// Искомая строка
    /// </summary>
    public string? FindText { get; set; }
}