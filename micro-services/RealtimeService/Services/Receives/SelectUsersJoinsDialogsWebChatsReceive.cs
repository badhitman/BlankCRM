////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.realtime;

/// <summary>
/// SelectUsersJoinsDialogsWebChats
/// </summary>
public class SelectUsersJoinsDialogsWebChatsReceive(IWebChatService webChatRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectUsersJoinsDialogsWebChatsRequestModel>?, TPaginationResponseStandardModel<UserJoinDialogWebChatModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectUsersJoinsDialogsWebChatsReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<UserJoinDialogWebChatModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectUsersJoinsDialogsWebChatsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.SelectUsersJoinsDialogsWebChatsAsync(req, token);
    }
}


/*
 * Task<ResponseBaseModel> UpdateDialogWebChatAdminAsync(TAuthRequestStandardModel<DialogWebChatBaseModel>
 /// <summary>
/// 
/// </summary>
public class Receive(IWebChatService webChatRepo)
    : IResponseReceive<?, ?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.Receive;

    /// <inheritdoc/>
    public async Task<?> ResponseHandleActionAsync(? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.(req, token);
    }
}
 */


/*
 * Task<ResponseBaseModel> UpdateDialogWebChatInitiatorAsync(TAuthRequestStandardModel<DialogWebChatBaseModel>
 /// <summary>
/// 
/// </summary>
public class Receive(IWebChatService webChatRepo)
    : IResponseReceive<?, ?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.Receive;

    /// <inheritdoc/>
    public async Task<?> ResponseHandleActionAsync(? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.(req, token);
    }
}
 */


/*
 * Task<ResponseBaseModel> UpdateMessageWebChatAsync(TAuthRequestStandardModel<MessageWebChatModelDB>
 /// <summary>
/// 
/// </summary>
public class Receive(IWebChatService webChatRepo)
    : IResponseReceive<?, ?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.Receive;

    /// <inheritdoc/>
    public async Task<?> ResponseHandleActionAsync(? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.(req, token);
    }
}
 */


/*
 * Task<ResponseBaseModel> UserInjectDialogWebChatAsync(TAuthRequestStandardModel<UserInjectDialogWebChatRequestModel>
 /// <summary>
/// 
/// </summary>
public class Receive(IWebChatService webChatRepo)
    : IResponseReceive<?, ?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.Receive;

    /// <inheritdoc/>
    public async Task<?> ResponseHandleActionAsync(? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.(req, token);
    }
}
 */