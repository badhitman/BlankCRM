////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public partial interface ICommerceServiceBase
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> FilesForGoodSetAsync(TAuthRequestStandardModel<FilesForGoodSetRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<FileGoodsConfigModelDB>> FilesForGoodSelectAsync(TPaginationRequestStandardModel<FilesForGoodSelectRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> FileForGoodUpdateAsync(TAuthRequestStandardModel<FileGoodsConfigModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> MoveFileForGoodsAsync(TAuthRequestStandardModel<MoveMetaObjectModel> req, CancellationToken token = default);
}