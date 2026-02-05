////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IEventsWebChatsNotifies
/// </summary>
public interface IEventsWebChatsNotifies
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> NewMessageWebChatHandle(NewMessageWebChatEventModel req, CancellationToken cancellationToken = default);
}
