////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// UserInjectDialogWebChatRequestModel
/// </summary>
public class UserInjectDialogWebChatRequestModel : UserInjectDialogWebChatBaseModel
{
    /// <summary>
    /// Подключиться только в случае если ни кто ещё не подключился
    /// </summary>
    /// <remarks>
    /// Взять чат в работу. Если кто-то уже взял в работу указанный чат, тогда туда можно отдельно подключиться, н оне в режиме эксклюзивности
    /// </remarks>
    public bool IsExclusiveJoin { get; set; }
}