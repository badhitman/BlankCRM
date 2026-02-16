////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// IRubricsService
/// </summary>
public interface IRubricsService
{
    /// <summary>
    /// RubricMove
    /// </summary>
    public Task<ResponseBaseModel> RubricMoveAsync(TAuthRequestStandardModel<RowMoveModel> req, CancellationToken token = default);

    /// <summary>
    /// Создать или обновить
    /// </summary>
    public Task<TResponseModel<int>> RubricCreateOrUpdateAsync(RubricStandardModel issueTheme, CancellationToken token = default);

    /// <summary>
    /// Получить под-рубрики (вложенные в рубрику). Если не указано, то root перечень
    /// </summary>
    public Task<List<RubricNestedModel>> RubricsChildListAsync(RubricsListRequestStandardModel req, CancellationToken token = default);

    /// <summary>
    /// Rubric read with parents hierarchy
    /// </summary>
    public Task<TResponseModel<List<RubricStandardModel>>> RubricReadWithParentsHierarchyAsync(int rubricId, CancellationToken token = default);

    /// <summary>
    /// Получить рубрики
    /// </summary>
    public Task<TResponseModel<List<RubricStandardModel>>> RubricsGetAsync(int[] rubricsIds, CancellationToken token = default);
}