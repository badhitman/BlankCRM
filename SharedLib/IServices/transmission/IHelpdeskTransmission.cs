////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IHelpdeskTransmission
/// </summary>
public interface IHelpdeskTransmission : IArticlesService, IHelpdeskServiceBase
{
    #region issue
    /// <summary>
    /// Добавить событие в журнал
    /// </summary>
    public Task<TResponseModel<bool>> PulsePushAsync(PulseRequestModel req, bool waitResponse = true, CancellationToken token = default);
    #endregion
}