////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Identity
/// </summary>
public interface IIdentityTransmission : IIdentityTools
{
    /// <summary>
    /// Отправка Email
    /// </summary>
    public Task<ResponseBaseModel> SendEmailAsync(SendEmailRequestModel req, bool waitResponse = true, CancellationToken token = default);
}