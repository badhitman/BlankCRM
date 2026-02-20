////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// NomenclatureScopeModel
/// </summary>
public class NomenclatureScopeModel
{
    /// <inheritdoc/>
    public string? Name { get; set; }

    /// <summary>
    /// Описание/примечание для объекта
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Объект отключён
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Базовая единица измерения `Номенклатуры`
    /// </summary>
    public required string BaseUnit { get; set; }

    /// <summary>
    /// Торговые предложения по Номенклатуре
    /// </summary>
    public required List<OfferScopeModel> Offers { get; set; }
}