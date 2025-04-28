////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Запрос поиска с указанием владельца
/// </summary>
public class FindWithOwnedRequestModel : SimplePaginationRequestModel
{
    /// <summary>
    /// Идентификатор владельца
    /// </summary>
    public string? OwnerId { get; set; }
}