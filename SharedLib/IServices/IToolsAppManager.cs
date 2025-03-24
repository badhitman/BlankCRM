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
    public Task<ApiRestConfigModelDB[]> GetAllConfigurations(CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ApiRestConfigModelDB> ReadConfiguration(int confId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<int>> UpdateOrCreateConfig(ApiRestConfigModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteConfig(int confId, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<SyncDirectoryModelDB[]> GetSyncDirectoriesForConfig(int confId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<SyncDirectoryModelDB> ReadSyncDirectory(int syncDirId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateOrCreateSyncDirectory(SyncDirectoryModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteSyncDirectory(int syncDirectoryId, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<ExeCommandModelDB[]> GetExeCommandsForConfig(int confId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ExeCommandModelDB> ReadExeCommand(int comId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateOrCreateExeCommand(ExeCommandModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteExeCommand(int exeCommandId, CancellationToken token = default);
}