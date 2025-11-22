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
public interface IRubricsTransmission : IRubricsBaseService
{
    /// <summary>
    /// Создать тему для обращений
    /// </summary>
    public Task<TResponseModel<int>> RubricCreateOrUpdateAsync(RubricStandardModel issueTheme, CancellationToken token = default);

    /// <summary>
    /// Сдвинуть рубрику
    /// </summary>
    public Task<ResponseBaseModel> RubricMoveAsync(TAuthRequestModel<RowMoveModel> req, CancellationToken token = default);

    /// <summary>
    /// Получить рубрики
    /// </summary>
    public Task<TResponseModel<List<RubricStandardModel>>> RubricsGetAsync(IEnumerable<int> rubricsIds, CancellationToken token = default);
}