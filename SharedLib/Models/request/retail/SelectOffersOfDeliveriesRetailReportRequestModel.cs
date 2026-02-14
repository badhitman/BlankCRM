////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectOffersOfDeliveriesRetailReportRequestModel
/// </summary>
public class SelectOffersOfDeliveriesRetailReportRequestModel : SelectRetailDocumentsBaseModel
{
    /// <inheritdoc/>
    public List<DeliveryStatusesEnum?>? StatusesFilter { get; set; }

    /// <inheritdoc/>
    public List<int?>? TypesFilter { get; set; }
}