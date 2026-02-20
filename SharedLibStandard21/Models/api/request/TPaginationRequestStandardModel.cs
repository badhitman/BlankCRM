////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Запрос с пагинацией
/// </summary>
public class TPaginationRequestStandardModel<T> : SimplePaginationRequestStandardModel
{
    /// <summary>
    /// Payload
    /// </summary>
    public virtual T? Payload { get; set; }
}