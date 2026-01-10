////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AttendanceRecordsDeleteResponseModel
/// </summary>
public class AttendanceRecordsDeleteResponseModel
{

    /// <inheritdoc/>
    public required OfferModelDB Offer { get; set; }

    /// <inheritdoc/>
    public required RecordsAttendanceModelDB[] Records { get; set; }
}
