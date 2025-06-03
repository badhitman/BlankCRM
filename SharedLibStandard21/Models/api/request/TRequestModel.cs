////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Авторизованный запрос (от имени пользователя)
/// </summary>
public class TRequestModel<T>
{
    /// <summary>
    /// Request
    /// </summary>
    public T Payload { get; set; }
}