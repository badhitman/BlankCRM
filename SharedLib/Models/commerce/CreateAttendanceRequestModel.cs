////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CreateAttendanceRequestModel
/// </summary>
public class CreateAttendanceRequestModel
{
    /// <summary>
    /// Records
    /// </summary>
    public required List<WorkScheduleBaseModel> Records { get; set; }

    /// <summary>
    /// Торговое предложение
    /// </summary>
    public required int OfferId { get; set; }
}
