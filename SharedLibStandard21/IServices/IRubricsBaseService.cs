////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// IRubricsBaseService
/// </summary>
public interface IRubricsBaseService
{
    /// <summary>
    /// Получить под-рубрики (вложенные в рубрику). Если не указано, то root перечень
    /// </summary>
    public Task<List<UniversalBaseModel>> RubricsChildListAsync(RubricsListRequestStandardModel req, CancellationToken token = default);

    /// <summary>
    /// Rubric read with parents hierarchy
    /// </summary>
    public Task<TResponseModel<List<RubricStandardModel>>> RubricReadWithParentsHierarchyAsync(int rubricId, CancellationToken token = default);
}
