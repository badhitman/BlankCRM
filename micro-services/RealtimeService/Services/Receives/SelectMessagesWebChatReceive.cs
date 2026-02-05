////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.realtime;

/// <summary>
/// SelectMessagesWebChat
/// </summary>
public class SelectMessagesWebChatReceive(IWebChatService webChatRepo)
    : IResponseReceive<SelectMessagesForWebChatRequestModel?, TResponseModel<SelectMessagesForWebChatResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectMessagesWebChatReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<SelectMessagesForWebChatResponseModel>?> ResponseHandleActionAsync(SelectMessagesForWebChatRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.SelectMessagesWebChatAsync(req, token);
    }
}


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