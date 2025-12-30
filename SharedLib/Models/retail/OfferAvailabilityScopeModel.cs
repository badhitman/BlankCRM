////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// OfferAvailabilityScopeModel
/// </summary>
public class OfferAvailabilityScopeModel
{
    /// <summary>
    /// Склад
    /// </summary>
    public required string Warehouse { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public decimal Quantity { get; set; }
}