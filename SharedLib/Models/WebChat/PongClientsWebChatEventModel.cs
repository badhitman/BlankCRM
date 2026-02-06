////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Ответ на пинг (опрос/перекличка). Кто сейчас онлайн (подключён)
/// </summary>
/// <remarks>
/// Когда инициируется пинг <see cref="PingClientsWebChatEventModel"/> - в теле запроса указывается имя топика для ответа.
/// В этот топик все клиенты (онлайн) отправляют информацию о себе.
/// </remarks>
public class PongClientsWebChatEventModel
{
    /// <inheritdoc/>
    public required UserInfoModel? CurrentUserSession { get; set; }

    /// <inheritdoc/>
    public required string ResponseContainerGUID { get; set; }

    /// <inheritdoc/>
    public required string SenderContainerGUID { get; set; }
}