////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// КЛАДР 4.0
/// </summary>
public interface IKladrService
{
    /// <summary>
    /// Очистить таблицу временных данных КЛАДР
    /// </summary>
    public Task<ResponseBaseModel> ClearTempKladrAsync(CancellationToken token = default);

    /// <summary>
    /// Состояние хранилища КЛАДР
    /// </summary>
    public Task<MetadataKladrModel> GetMetadataKladrAsync(GetMetadataKladrRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Загрузить порцию данных КЛАДР 4.0
    /// </summary>
    public Task<ResponseBaseModel> UploadPartTempKladrAsync(UploadPartTableDataModel req, CancellationToken token = default);

    /// <summary>
    /// Транзит данных из временного хранилища в прод
    /// </summary>
    public Task<ResponseBaseModel> FlushTempKladrAsync(CancellationToken token = default);
}