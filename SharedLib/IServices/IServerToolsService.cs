////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IServerToolsService
/// </summary>
public interface IServerToolsService
{
    /// <summary>
    /// ExeCommand
    /// </summary>
    public Task<TResponseModel<string>> ExeCommandAsync(ExeCommandModelDB req, CancellationToken token = default);

    /// <summary>
    /// GetDirectory
    /// </summary>
    public Task<TResponseModel<List<ToolsFilesResponseModel>>> GetDirectoryDataAsync(ToolsFilesRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Существует директория
    /// </summary>
    public Task<ResponseBaseModel> DirectoryExistAsync(string remoteDirectory, CancellationToken token = default);

    /// <summary>
    /// Обновить файл (или создать, если его нет)
    /// </summary>
    /// <remarks>
    /// Если файла не существует, то создаёт его. В противном случае перезаписывает/обновляет.
    /// </remarks>
    public Task<TResponseModel<string>> UpdateFileAsync(string fileScopeName, string remoteDirectory, byte[] bytes, CancellationToken token = default);

    /// <summary>
    /// UpdateFile
    /// </summary>
    public Task<TResponseModel<bool>> DeleteFileAsync(DeleteRemoteFileRequestModel req, CancellationToken token = default);
}