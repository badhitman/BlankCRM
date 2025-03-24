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
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRow(TPaginationRequestModel<int> req, CancellationToken token = default)
        => await storeRepo.GoToPageForRow(req, token);

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelect(TPaginationRequestModel<LogsSelectRequestModel> req, CancellationToken token = default)
        => await storeRepo.LogsSelect(req, token);

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogs(PeriodDatesTimesModel req, CancellationToken token = default)
        => await storeRepo.MetadataLogs(req, token);
}