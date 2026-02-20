////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Registers select request base
/// </summary>
public class RegistersSelectRequestBaseModel
{
    /// <summary>
    /// Склады
    /// </summary>
    public int[]? WarehousesFilter { get; set; }

    /// <summary>
    /// Фильтр по номенклатуре
    /// </summary>
    public int[]? NomenclatureFilter { get; set; }

    /// <summary>
    /// Фильтр по коммерческому предложению
    /// </summary>
    public int[]? OfferFilter { get; set; }

    /// <summary>
    /// Min quantity
    /// </summary>
    public int? MinQuantity { get; set; }
}
