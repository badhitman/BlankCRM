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
    public Task<TResponseModel<ManageManufactureModelDB>> ReadManufactureConfig(int projectId, string userId, CancellationToken token = default);

    /// <summary>
    /// Обновить конфигурацию генератора кода
    /// </summary>
    public Task<ResponseBaseModel> UpdateManufactureConfig(ManageManufactureModelDB manufacture, CancellationToken token = default);

    /// <summary>
    /// Установить системное имя сущности.
    /// </summary>
    /// <remarks>
    /// Если установить null (или пустую строку), тогда значение удаляется
    /// </remarks>
    public Task<ResponseBaseModel> SetOrDeleteSystemName(UpdateSystemNameModel request, CancellationToken token = default);

    /// <summary>
    /// Получить системные имена для генератора кода
    /// </summary>
    public Task<List<SystemNameEntryModel>> GetSystemNames(int manufactureId, CancellationToken token = default);

    /// <summary>
    /// Create snapshot
    /// </summary>
    public Task CreateSnapshot(StructureProjectModel dump, int projectId, string name, CancellationToken token = default);
}