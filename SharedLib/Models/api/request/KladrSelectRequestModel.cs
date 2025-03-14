////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// KladrSelectRequestModel
/// </summary>
public class KladrSelectRequestModel : PaginationRequestModel
{
    /// <summary>
    /// Применение фильтра к коду
    /// </summary>
    public string? CodeLikeFilter { get; set; }
}