////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IClientHTTPRestService
/// </summary>
public interface IClientHTTPRestService : IServerToolsService, IKladrService
{
    /// <summary>
    /// GetMe
    /// </summary>
    public Task<TResponseModel<ExpressProfileResponseModel>> GetMe(CancellationToken cancellationToken = default);

    #region files (ext)
    /// <summary>
    /// Создать сессию порционной (частями) загрузки файлов
    /// </summary>
    public Task<TResponseModel<PartUploadSessionModel>> FilePartUploadSessionStart(PartUploadSessionStartRequestModel req);

    /// <summary>
    /// Загрузка порции файла
    /// </summary>
    public Task<ResponseBaseModel> FilePartUpload(SessionFileRequestModel req);
    #endregion
}