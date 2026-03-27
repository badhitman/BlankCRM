////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// FirebaseService
/// </summary>
public interface IFirebaseServiceTransmission : IFirebaseService
{
    /// <inheritdoc/>
    public Task<TResponseModel<FirebaseSDKConfigModel>> GetFirebaseConfigAsync(CancellationToken token = default);
}