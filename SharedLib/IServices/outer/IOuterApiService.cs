////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IOuterApiService
/// </summary>
public interface IOuterApiService
{
    /// <summary>
    /// Скачать данные и сохранить в БД
    /// </summary>
    public Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default);

    /// <summary>
    /// Получить задания-очереди которые в данный момент в работе
    /// </summary>
    public Task<TResponseModel<List<RabbitMqManagementResponseModel>>> HealthCheckAsync(CancellationToken token = default);
}