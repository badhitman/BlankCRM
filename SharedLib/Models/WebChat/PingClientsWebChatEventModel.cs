////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Перекличка/Опрос. Кто сейчас онлайн (подключён)
/// </summary>
/// <remarks>
/// Все клиенты подписываются на такое событие, что бы дать обратную связь.
/// Все подключённые/подписанные клиенты сети отвечают в указанный топик информацию о себе <see cref="UserInfoModel"/>
/// </remarks>
public class PingClientsWebChatEventModel
{
    /// <inheritdoc/>
    public required string LayoutContainerId { get; set; }
}
