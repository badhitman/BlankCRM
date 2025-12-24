////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectOffersOfOrdersRetailReportRequestModel
/// </summary>
public class SelectOffersOfOrdersRetailReportRequestModel : SelectRetailDocumentsBaseModel
{
    /// <inheritdoc/>
    public List<StatusesDocumentsEnum?>? StatusesFilter { get; set; }

    /// <inheritdoc/>
    public int NumWeekOfYear { get; set; }
}