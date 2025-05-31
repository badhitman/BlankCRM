////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Производство кода C#
/// </summary>
public interface IManufactureService
{
    /// <summary>
    /// Прочитать конфигурацию генератора кода
    /// </summary>
    public Task<TResponseModel<ManageManufactureModelDB?>> ReadManufactureConfigAsync(int projectId, string userId, CancellationToken token = default);

    /// <summary>
    /// Обновить конфигурацию генератора кода
    /// </summary>
    public Task<ResponseBaseModel> UpdateManufactureConfigAsync(ManageManufactureModelDB manufacture, CancellationToken token = default);

    /// <summary>
    /// Установить системное имя сущности.
    /// </summary>
    /// <remarks>
    /// Если установить null (или пустую строку), тогда значение удаляется
    /// </remarks>
    public Task<ResponseBaseModel> SetOrDeleteSystemNameAsync(UpdateSystemNameModel request, CancellationToken token = default);

    /// <summary>
    /// Получить системные имена для генератора кода
    /// </summary>
    public Task<List<SystemNameEntryModel>> GetSystemNamesAsync(int manufactureId, CancellationToken token = default);

    /// <summary>
    /// Create snapshot
    /// </summary>
    public Task CreateSnapshotAsync(StructureProjectModel dump, int projectId, string name, CancellationToken token = default);
}