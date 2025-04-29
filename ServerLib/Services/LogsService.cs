////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace ServerLib;

/// <summary>
/// LogsService
/// </summary>
public class LogsService(IStorageTransmission storeRepo) : ILogsService
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRowAsync(TPaginationRequestStandardModel<int> req, CancellationToken token = default)
        => await storeRepo.GoToPageForRowAsync(req, token);

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestStandardModel<LogsSelectRequestModel> req, CancellationToken token = default)
        => await storeRepo.LogsSelectAsync(req, token);

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken token = default)
        => await storeRepo.MetadataLogsAsync(req, token);
}