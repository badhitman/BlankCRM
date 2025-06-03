////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// IRubricsTransmission
/// </summary>
public interface IRubricsTransmission
{
    /// <summary>
    /// Получить темы обращений
    /// </summary>
    public Task<List<UniversalBaseModel>> RubricsListAsync(RubricsListRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Создать тему для обращений
    /// </summary>
    public Task<TResponseModel<int>> RubricCreateOrUpdateAsync(RubricStandardModel issueTheme, CancellationToken token = default);

    /// <summary>
    /// Сдвинуть рубрику
    /// </summary>
    public Task<ResponseBaseModel> RubricMoveAsync(TAuthRequestModel<RowMoveModel> req, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные рубрики (вместе со всеми вышестоящими родителями)
    /// </summary>
    public Task<TResponseModel<List<RubricStandardModel>>> RubricReadAsync(int rubricId, CancellationToken token = default);

    /// <summary>
    /// Получить рубрики
    /// </summary>
    public Task<TResponseModel<List<RubricStandardModel>>> RubricsGetAsync(IEnumerable<int> rubricsIds, CancellationToken token = default);
}