////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// IRubricsBaseService
/// </summary>
public interface IRubricsBaseService
{
    /// <summary>
    /// Получить рубрики, вложенные в рубрику (если не указано, то root перечень)
    /// </summary>
    public Task<List<UniversalBaseModel>> RubricsListAsync(RubricsListRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Rubric read with parents hierarchy
    /// </summary>
    public Task<TResponseModel<List<RubricStandardModel>>> RubricReadWithParentsHierarchyAsync(int rubricId, CancellationToken token = default);
}
