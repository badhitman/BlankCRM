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
    public async Task<TPaginationResponseStandardModel<NLogRecordModelDB>> GoToPageForRowLogsAsync(TPaginationRequestStandardModel<GoToPageForRowLogsRequestModel> req, CancellationToken token = default)
        => await storeRepo.GoToPageForRowLogsAsync(req, token);

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestStandardModel<LogsSelectRequestModel> req, CancellationToken token = default)
        => await storeRepo.LogsSelectAsync(req, token);

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken token = default)
        => await storeRepo.MetadataLogsAsync(req, token);
}