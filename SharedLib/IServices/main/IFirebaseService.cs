////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Firebase service
/// </summary>
public interface IFirebaseService
{
    /// <summary>
    /// Send Firebase Message
    /// </summary>
    public Task<TResponseModel<SendFirebaseMessageResultModel>> SendFirebaseNotificationAsync(TAuthRequestStandardModel<SendFirebaseMessageRequestModel> req, CancellationToken token = default);
}