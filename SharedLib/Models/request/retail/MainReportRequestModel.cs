////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// MainReportRequestModel
/// </summary>
public class MainReportRequestModel : PeriodBaseModel
{
    /// <inheritdoc/>
    public int NumWeekOfYear { get; set; }

    /// <inheritdoc/>
    public int SelectedYear { get; set; }
}