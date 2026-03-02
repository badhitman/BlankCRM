////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IHelpDeskTransmission
/// </summary>
public interface IHelpDeskTransmission : IArticlesService, IHelpDeskServiceBase
{
    #region issue
    /// <summary>
    /// Добавить событие в журнал
    /// </summary>
    public Task<TResponseModel<bool>> PulsePushAsync(PulseRequestModel req, bool waitResponse = true, CancellationToken token = default);
    #endregion
}