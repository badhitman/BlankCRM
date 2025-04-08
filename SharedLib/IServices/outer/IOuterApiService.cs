////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IOuterApiBaseService
/// </summary>
public interface IOuterApiService
{
    /// <summary>
    /// DownloadAndSaveAsync
    /// </summary>
    public Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default);
}

//public abstract class 