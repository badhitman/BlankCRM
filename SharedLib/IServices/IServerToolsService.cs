﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IServerToolsService
/// </summary>
public interface IServerToolsService
{
    #region files (base)
    /// <summary>
    /// ExeCommand
    /// </summary>
    public Task<TResponseModel<string>> ExeCommand(ExeCommandModelDB req);

    /// <summary>
    /// GetDirectory
    /// </summary>
    public Task<TResponseModel<List<ToolsFilesResponseModel>>> GetDirectoryData(ToolsFilesRequestModel req);

    /// <summary>
    /// Существует директория
    /// </summary>
    public Task<ResponseBaseModel> DirectoryExist(string remoteDirectory);

    /// <summary>
    /// Обновить файл (или создать, если его нет)
    /// </summary>
    /// <remarks>
    /// Если файла не существует, то создаёт его. В противном случае перезаписывает/обновляет.
    /// </remarks>
    public Task<TResponseModel<string>> UpdateFile(string fileScopeName, string remoteDirectory, byte[] bytes);

    /// <summary>
    /// UpdateFile
    /// </summary>
    public Task<TResponseModel<bool>> DeleteFile(DeleteRemoteFileRequestModel req);
    #endregion
}