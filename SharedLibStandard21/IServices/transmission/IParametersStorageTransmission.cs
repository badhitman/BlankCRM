////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// IParametersStorageTransmission
/// </summary>
public interface IParametersStorageTransmission
{
    /// <summary>
    /// Прочитать значение параметра. null - если значения нет
    /// </summary>
    /// <typeparam name="T">Тип данных (для десериализации из JSON)</typeparam>
    /// <remarks>
    /// Возвращается самое актуальное значение (последнее установленное). Хранится история значений - если значение будет часто меняться будет ротация стека накопленных значений с усечением от 150 до 100.
    /// Проверка переполнения происходит при каждой команде сохранения.
    /// </remarks>
    public Task<TResponseModel<T?>> ReadParameterAsync<T>(StorageMetadataModel req, CancellationToken token = default);

    /// <summary>
    /// Прочитать значения параметров. Данные запрашиваемых параметров, которые отсутствуют в БД - не попадут в возвращаемый ответ.
    /// </summary>
    /// <typeparam name="T">Тип данных (для десериализации из JSON)</typeparam>
    /// <remarks>
    /// Возвращается самое актуальное значение (последнее установленное). Хранится история значений - если значение будет часто меняться будет ротация стека накопленных значений с усечением от 150 до 100.
    /// Проверка переполнения происходит при каждой команде сохранения.
    /// </remarks>
    public Task<TResponseModel<List<T?>?>> ReadParametersAsync<T>(StorageMetadataModel[] req, CancellationToken token = default);

    /// <summary>
    /// Сохранить параметр
    /// </summary>
    public Task<TResponseModel<int>> SaveParameterAsync<T>(T payload_query, StorageMetadataModel store, bool trim, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Удалить параметр
    /// </summary>
    public Task<TResponseModel<int>> DeleteParameterAsync(StorageMetadataModel key, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Найти/подобрать значения параметров (со всей историей значений)
    /// </summary>
    public Task<TResponseModel<T?[]?>> FindParametersAsync<T>(RequestStorageBaseModel req, CancellationToken token = default);

    #region tag`s
    /// <summary>
    /// TagSet
    /// </summary>
    public Task<TResponseModel<bool>> TagSetAsync(TagSetModel req, CancellationToken token = default);

    /// <summary>
    /// TagsSelect
    /// </summary>
    public Task<TPaginationResponseModel<TagViewModel>> TagsSelectAsync(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default);
    #endregion
}