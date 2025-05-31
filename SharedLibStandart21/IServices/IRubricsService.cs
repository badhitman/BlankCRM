////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// HelpDesk (service)
/// </summary>
public interface IRubricsService
{
    /// <summary>
    /// Получить рубрики, вложенные в рубрику (если не указано, то root перечень)
    /// </summary>
    public Task<List<UniversalBaseModel>> RubricsListAsync(RubricsListRequestModel req, CancellationToken token = default);

    /// <summary>
    /// RubricMove
    /// </summary>
    public Task<ResponseBaseModel> RubricMoveAsync(TAuthRequestModel<RowMoveModel> req, CancellationToken token = default);

    /// <summary>
    /// Rubric create (or update)
    /// </summary>
    public Task<TResponseModel<int>> RubricCreateOrUpdateAsync(RubricStandardModel req, CancellationToken token = default);

    /// <summary>
    /// Rubric read
    /// </summary>
    public Task<TResponseModel<List<RubricStandardModel>>> RubricReadAsync(int rubricId, CancellationToken token = default);

    /// <summary>
    /// Rubrics get
    /// </summary>
    public Task<TResponseModel<List<RubricStandardModel>>> RubricsGetAsync(int[] rubricsIds, CancellationToken token = default);
}