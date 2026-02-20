////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// OfferAvailabilityModelDB
/// </summary>
[Index(nameof(WarehouseId), nameof(OfferId), IsUnique = true), Index(nameof(Quantity))]
public class OfferAvailabilityModelDB : RowOfSimpleDocumentModel
{
    /// <summary>
    /// Склад
    /// </summary>
    public required int WarehouseId { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Organization
    /// </summary>
    public OrganizationModelDB? Organization { get; set; }
    /// <summary>
    /// Organization
    /// </summary>
    public int? OrganizationId { get; set; }


    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{base.ToString()} ({nameof(Quantity)}:{Quantity})";
    }
}