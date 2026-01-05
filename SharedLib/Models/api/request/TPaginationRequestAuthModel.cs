////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Запрос с пагинацией (авторизованный)
/// </summary>
public class TPaginationRequestAuthModel<T> : PaginationRequestStandardModel
{
    /// <summary>
    /// Payload
    /// </summary>
    public required T Payload { get; set; }

    /// <summary>
    /// Пользователь, который отправил запрос (id Identity)
    /// </summary>
    public required string SenderActionUserId { get; set; }
}
