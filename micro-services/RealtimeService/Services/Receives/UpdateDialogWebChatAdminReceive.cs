////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.realtime;

/// <summary>
/// UpdateDialogWebChatAdmin
/// </summary>
public class UpdateDialogWebChatAdminReceive(IWebChatService webChatRepo)
    : IResponseReceive<TAuthRequestStandardModel<DialogWebChatBaseModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateDialogWebChatAdminReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<DialogWebChatBaseModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.UpdateDialogWebChatAdminAsync(req, token);
    }
}


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