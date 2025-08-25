////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Запрос с пагинацией
/// </summary>
public class TPaginationRequestModel<T> : TPaginationRequestStandardModel<T>
{
    /// <summary>
    /// Payload
    /// </summary>
    public new T? Payload { get; set; }
}