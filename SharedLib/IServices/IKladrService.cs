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
    /// Очистить таблицу временных даннык КЛАДР
    /// </summary>
    public Task<ResponseBaseModel> ClearTempKladr(CancellationToken token = default);

    /// <summary>
    /// Состояние хранилища КЛАДР
    /// </summary>
    public Task<MetadataKladrModel> GetMetadataKladr(GetMetadataKladrRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Загрузить порцию данных КЛАДР 4.0
    /// </summary>
    public Task<ResponseBaseModel> UploadPartTempKladr(UploadPartTableDataModel req, CancellationToken token = default);

    /// <summary>
    /// Транзит данных из временного хранилища в прод
    /// </summary>
    public Task<ResponseBaseModel> FlushTempKladr(CancellationToken token = default);
}