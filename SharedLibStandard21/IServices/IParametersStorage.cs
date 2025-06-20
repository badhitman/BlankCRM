////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// Хранилище параметров приложений
/// </summary>
/// <remarks>
/// Значения/данные сериализуются в JSON строку при сохранении и десерализируются при чтении
/// </remarks>
public interface IParametersStorage
{
    #region tag`s
    /// <summary>
    /// TagSet
    /// </summary>
    public Task<ResponseBaseModel> TagSetAsync(TagSetModel req, CancellationToken token = default);

    /// <summary>
    /// TagsSelect
    /// </summary>
    public Task<TPaginationResponseModel<TagViewModel>> TagsSelectAsync(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default);
    #endregion

    #region parameter`s
    /// <summary>
    /// Сохранить параметр
    /// </summary>
    public Task SaveAsync<T>(T obj, StorageMetadataModel set, bool trimHistory = false, CancellationToken token = default);

    /// <summary>
    /// Прочитать значение параметра. null - если значения нет
    /// </summary>
    /// <typeparam name="T">Тип данных (для десериализации из JSON)</typeparam>
    /// <remarks>
    /// Возвращается самое актуальное значение (последнее установленное). Хранится история значений - если значение будет часто меняться будет ротация стека накопленных значений с усечением от 150 до 100.
    /// Проверка переполнения происходит при каждой команде сохранения.
    /// </remarks>
    public Task<T> ReadAsync<T>(StorageMetadataModel req, CancellationToken token = default);

    /// <summary>
    /// Поиск значений параметров
    /// </summary>
    public Task<FundedParametersModel<T>[]> FindAsync<T>(FindStorageBaseModel req, CancellationToken token = default);

    /// <summary>
    /// FlushParameterAsync
    /// </summary>
    public Task<TResponseModel<int?>> FlushParameterAsync(StorageCloudParameterViewModel storage, bool trimHistory = false, CancellationToken token = default);

    /// <summary>
    /// Прочитать значение параметра. null - если значения нет
    /// </summary>
    /// <remarks>
    /// Возвращается самое актуальное значение (последнее установленное). Хранится история значений - если значение будет часто меняться будет ротация стека накопленных значений с усечением от 150 до 100.
    /// Проверка переполнения происходит при каждой команде сохранения.
    /// </remarks>
    public Task<TResponseModel<StorageCloudParameterPayloadModel>> ReadParameterAsync(StorageMetadataModel req, CancellationToken token = default);

    /// <summary>
    /// Прочитать значения параметров. Данные запрашиваемых параметров, которые отсутствуют в БД - не попадут в возвращаемый ответ.
    /// </summary>
    /// <remarks>
    /// Возвращается самое актуальные значения (последнее установленное). Хранится история значений - если значение будет часто меняться будет ротация стека накопленных значений с усечением от 150 до 100.
    /// Проверка переполнения происходит при каждой команде сохранения.
    /// </remarks>
    public Task<TResponseModel<List<StorageCloudParameterPayloadModel>>> ReadParametersAsync(StorageMetadataModel[] req, CancellationToken token = default);

    /// <summary>
    /// Поиск значений параметров (JSON данные)
    /// </summary>
    public Task<TResponseModel<FoundParameterModel[]>> FindRawAsync(FindStorageBaseModel req, CancellationToken token = default);
    #endregion
}