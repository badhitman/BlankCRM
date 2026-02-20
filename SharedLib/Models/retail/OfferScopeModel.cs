////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// OfferScopeModel
/// </summary>
public class OfferScopeModel
{
    /// <summary>
    /// Объект отключён
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <inheritdoc/>
    public virtual string? Name { get; set; }

    /// <summary>
    /// Описание/примечание для объекта
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Короткое название
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    /// Шаблон допустимых значений через пробел
    /// </summary>
    public string? QuantitiesTemplate { get; set; }

    /// <summary>
    /// Остатки
    /// </summary>
    public required List<OfferAvailabilityScopeModel> Registers { get; set; }

    /// <summary>
    /// Единица измерения предложения
    /// </summary>
    public required string OfferUnit { get; set; }

    /// <summary>
    /// Кратность к базовой единице товара
    /// </summary>
    public decimal Multiplicity { get; set; }

    /// <summary>
    /// Цена за единицу <see cref="OfferUnit"/>
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Вес
    /// </summary>
    public decimal Weight { get; set; }
}