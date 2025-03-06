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
    public Task<ResponseBaseModel> ClearTempKladr();

    /// <summary>
    /// Состояние хранилища КЛАДР
    /// </summary>
    public Task<MetadataKladrModel> GetMetadataKladr(GetMetadataKladrRequestModel req);

    /// <summary>
    /// Загрузить порцию данных КЛАДР 4.0
    /// </summary>
    public Task<ResponseBaseModel> UploadPartTempKladr(UploadPartTableDataModel req);
}