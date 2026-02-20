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
    public Task<ApiRestConfigModelDB[]> GetAllConfigurationsAsync(CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ApiRestConfigModelDB> ReadConfigurationAsync(int confId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<int>> UpdateOrCreateConfigAsync(ApiRestConfigModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteConfigAsync(int confId, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<SyncDirectoryModelDB[]> GetSyncDirectoriesForConfigAsync(int confId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<SyncDirectoryModelDB> ReadSyncDirectoryAsync(int syncDirId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateOrCreateSyncDirectoryAsync(SyncDirectoryModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteSyncDirectoryAsync(int syncDirectoryId, CancellationToken token = default);


    /// <inheritdoc/>
    public Task<ExeCommandModelDB[]> GetExeCommandsForConfigAsync(int confId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ExeCommandModelDB> ReadExeCommandAsync(int comId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateOrCreateExeCommandAsync(ExeCommandModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteExeCommandAsync(int exeCommandId, CancellationToken token = default);
}