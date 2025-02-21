////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Управление настройками для управления удалённого системами
/// </summary>
/// <remarks>
/// Токены, синхронизации, логи
/// </remarks>
public interface IToolsAppManager
{
    /// <inheritdoc/>
    public Task<ApiRestConfigModelDB[]> GetAllConfigurations();

    /// <inheritdoc/>
    public Task<ApiRestConfigModelDB> ReadConfiguration(int confId);

    /// <inheritdoc/>
    public Task<TResponseModel<int>> UpdateOrCreateConfig(ApiRestConfigModelDB req);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteConfig(int confId);


    /// <inheritdoc/>
    public Task<SyncDirectoryModelDB[]> GetSyncDirectoriesForConfig(int confId);

    /// <inheritdoc/>
    public Task<SyncDirectoryModelDB> ReadSyncDirectory(int syncDirId);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateOrCreateSyncDirectory(SyncDirectoryModelDB req);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteSyncDirectory(int syncDirectoryId);


    /// <inheritdoc/>
    public Task<ExeCommandModelDB[]> GetExeCommandsForConfig(int confId);

    /// <inheritdoc/>
    public Task<ExeCommandModelDB> ReadExeCommand(int comId);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateOrCreateExeCommand(ExeCommandModelDB req);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteExeCommand(int exeCommandId);
}