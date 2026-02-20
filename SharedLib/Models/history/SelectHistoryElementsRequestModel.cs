////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectTraceElementsRequestModel
/// </summary>
public class SelectHistoryElementsRequestModel : PeriodBaseModel
{
    /// <inheritdoc/>
    public int FilterId { get; set; }
}