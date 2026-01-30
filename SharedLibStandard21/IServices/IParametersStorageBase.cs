////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// IParametersStorageBase
/// </summary>
public interface IParametersStorageBase
{
    #region tag`s
    /// <summary>
    /// TagSet
    /// </summary>
    public Task<ResponseBaseModel> TagSetAsync(TagSetModel req, CancellationToken token = default);

    /// <summary>
    /// TagsSelect
    /// </summary>
    public Task<TPaginationResponseStandardModel<TagViewModel>> TagsSelectAsync(TPaginationRequestStandardModel<SelectMetadataRequestModel> req, CancellationToken token = default);
    #endregion
}
