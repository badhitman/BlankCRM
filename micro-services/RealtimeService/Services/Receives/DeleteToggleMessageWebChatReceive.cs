////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.realtime;

/// <summary>
/// DeleteToggleMessageWebChat
/// </summary>
public class DeleteToggleMessageWebChatReceive(IWebChatService webChatRepo)
    : IResponseReceive<TAuthRequestStandardModel<int>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteToggleMessageWebChatReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.DeleteToggleMessageWebChatAsync(req, token);
    }
}



/*
 * Task<ResponseBaseModel> DeleteUserJoinDialogWebChatAsync(TAuthRequestStandardModel<int>
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
 * Task<TResponseModel<List<DialogWebChatModelDB>>> DialogsWebChatsReadAsync(TAuthRequestStandardModel<int[]>
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
 * Task<TResponseModel<DialogWebChatModelDB>> InitWebChatSessionAsync(InitWebChatSessionRequestModel
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
 * Task<TPaginationResponseStandardModel<DialogWebChatModelDB>> SelectDialogsWebChatsAsync(TPaginationRequestStandardModel<SelectDialogsWebChatsRequestModel>
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
 * Task<TPaginationResponseStandardModel<MessageWebChatModelDB>> SelectMessagesForRoomWebChatAsync(TPaginationRequestAuthModel<SelectMessagesForWebChatRoomRequestModel>
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
 * Task<TResponseModel<SelectMessagesForWebChatResponseModel>> SelectMessagesWebChatAsync(SelectMessagesForWebChatRequestModel
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
 * Task<TPaginationResponseStandardModel<UserJoinDialogWebChatModelDB>> SelectUsersJoinsDialogsWebChatsAsync(TPaginationRequestStandardModel<SelectUsersJoinsDialogsWebChatsRequestModel>
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