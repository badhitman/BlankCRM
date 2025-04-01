////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IOuterApiBaseService
/// </summary>
public interface IOuterApiBaseService
{

}

/// <summary>
/// Breez.ru api
/// </summary>
public interface IBreezRuApiService: IOuterApiBaseService
{

}

/// <summary>
/// Daichi business api
/// </summary>
public interface IDaichiBusinessApiService: IOuterApiBaseService
{

}

/// <summary>
/// b2b rusklimat api
/// </summary>
public interface IRusklimatComApiService: IOuterApiBaseService
{

}

/// <summary>
/// КЛАДР 4.0
/// </summary>
public interface IKladrService
{
    /// <summary>
    /// Очистить таблицу временных даннык КЛАДР
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