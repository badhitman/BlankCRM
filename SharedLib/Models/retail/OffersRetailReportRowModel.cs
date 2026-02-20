////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// OffersRetailReportRowModel
/// </summary>
public class OffersRetailReportRowModel
{
    /// <summary>
    /// Offer
    /// </summary>
    public required OfferModelDB Offer { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public required decimal Sum { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public required decimal Count { get; set; }
}