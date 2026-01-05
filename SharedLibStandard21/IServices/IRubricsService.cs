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
public interface IRubricsService : IRubricsBaseService
{
    /// <summary>
    /// RubricMove
    /// </summary>
    public Task<ResponseBaseModel> RubricMoveAsync(TRequestStandardModel<RowMoveModel> req, CancellationToken token = default);

    /// <summary>
    /// Rubric create (or update)
    /// </summary>
    public Task<TResponseModel<int>> RubricCreateOrUpdateAsync(RubricStandardModel req, CancellationToken token = default);

    /// <summary>
    /// Rubrics get
    /// </summary>
    public Task<TResponseModel<List<RubricStandardModel>>> RubricsGetAsync(int[] rubricsIds, CancellationToken token = default);
}