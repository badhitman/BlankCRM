////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IOuterApiBaseService
/// </summary>
public interface IOuterApiBaseService
{
    /// <summary>
    /// 
    /// </summary>
    public Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default);
}