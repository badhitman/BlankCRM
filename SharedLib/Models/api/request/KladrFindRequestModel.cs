////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// KladrFindRequestModel
/// </summary>
public class KladrFindRequestModel : PaginationRequestStandardModel
{
    /// <summary>
    /// Применение фильтра к коду (SQL Inject as LIKE operator)
    /// </summary>
    public string[]? CodeLikeFilter { get; set; }

    /// <summary>
    /// Искомая строка
    /// </summary>
    public string? FindText { get; set; }

    /// <summary>
    /// Искать в домах или нет?
    /// </summary>
    /// <remarks>
    /// Поиск в домах имеет смысл только для кодов объектов, но не наименования
    /// </remarks>
    public required bool IncludeHouses { get; set; }
}