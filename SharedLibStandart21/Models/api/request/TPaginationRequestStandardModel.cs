////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Запрос с пагинацией
/// </summary>
public class TPaginationRequestStandardModel<T> : PaginationRequestModel
{
    /// <summary>
    /// Payload
    /// </summary>
    public virtual T? Payload { get; set; }
}