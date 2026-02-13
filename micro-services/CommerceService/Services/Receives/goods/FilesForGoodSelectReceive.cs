////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// FilesForGoodSelect
/// </summary>
public class FilesForGoodSelectReceive(ICommerceService commerceRepo)
    : IResponseReceive<TPaginationRequestStandardModel<FilesForGoodSelectRequestModel>?, TPaginationResponseStandardModel<FileGoodsConfigModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FilesForGoodSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<FileGoodsConfigModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<FilesForGoodSelectRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.FilesForGoodSelectAsync(req, token);
    }
}