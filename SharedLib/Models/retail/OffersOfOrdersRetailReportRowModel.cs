////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// OffersOfOrdersRetailReportRowModel
/// </summary>
public class OffersOfOrdersRetailReportRowModel
{
    /// <summary>
    /// Offer
    /// </summary>
    public required OfferModelDB Offer { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public required decimal AmountSum { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public required decimal CountSum { get; set; }
}