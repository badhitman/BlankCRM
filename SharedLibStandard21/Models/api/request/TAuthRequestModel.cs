////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Авторизованный запрос (от имени пользователя)
/// </summary>
public class TAuthRequestModel<T> : TRequestModel<T>
{
    /// <summary>
    /// Пользователь, который отправил запрос (id Identity)
    /// </summary>
    public string? SenderActionUserId { get; set; }
}