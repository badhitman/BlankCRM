////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Авторизованный запрос (от имени пользователя)
/// </summary>
public class TRequestStandardModel<T>
{
    /// <summary>
    /// Request
    /// </summary>
    public T? Payload { get; set; }
}