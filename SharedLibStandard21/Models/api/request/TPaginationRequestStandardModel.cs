////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Запрос с пагинацией
/// </summary>
public class TPaginationRequestStandardModel<T> : SimplePaginationRequestModel
{
    /// <summary>
    /// Payload
    /// </summary>
    public virtual T? Payload { get; set; }
}