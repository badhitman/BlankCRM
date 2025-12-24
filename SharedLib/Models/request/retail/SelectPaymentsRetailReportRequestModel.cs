////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectPaymentsRetailReportRequestModel
/// </summary>
public class SelectPaymentsRetailReportRequestModel : SelectPaymentsRetailBaseRequestModel
{
    /// <inheritdoc/>
    public string[]? FilterIdentityIds { get; set; }

    /// <inheritdoc/>
    public int NumWeekOfYear { get; set; }
}