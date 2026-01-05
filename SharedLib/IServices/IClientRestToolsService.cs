////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IClientRestToolsService
/// </summary>
public interface IClientRestToolsService : IServerToolsService, IKladrService
{
    /// <summary>
    /// GetMe
    /// </summary>
    public Task<TResponseModel<ExpressProfileResponseStandardModel>> GetMeAsync(CancellationToken cancellationToken = default);

    #region files (ext)
    /// <summary>
    /// Создать сессию порционной (частями) загрузки файлов
    /// </summary>
    public Task<TResponseModel<PartUploadSessionModel>> FilePartUploadSessionStartAsync(PartUploadSessionStartRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Загрузка порции файла
    /// </summary>
    public Task<ResponseBaseModel> FilePartUploadAsync(SessionFileRequestModel req, CancellationToken token = default);
    #endregion
}