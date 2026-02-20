////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Единицы
/// </summary>
public enum UnitsOfMeasurementEnum
{
    /// <summary>
    /// Штука
    /// </summary>
    [Description("шт.")]
    Thing = 0,

    /// <summary>
    /// Комплект
    /// </summary>
    [Description("Комплект")]
    Set = 20,

    /// <summary>
    /// Пачка
    /// </summary>
    [Description("Пачка")]
    Packs = 40,

    /// <summary>
    /// Литр
    /// </summary>
    [Description("Литр")]
    Liter = 60,

    /// <summary>
    /// Коробка
    /// </summary>
    [Description("Коробка")]
    Box = 80,

    /// <summary>
    /// Кг
    /// </summary>
    [Description("Кг")]
    Kilogram = 100,

    /// <summary>
    /// Бутылка
    /// </summary>
    [Description("Бутылка")]
    Bottle = 120,

    /// <summary>
    /// Кипа
    /// </summary>
    [Description("Кипа")]
    Stack = 140,

    /// <summary>
    /// Связка
    /// </summary>
    [Description("Связка")]
    Bunch = 160,

    /// <summary>
    /// Час
    /// </summary>
    [Description("Час")]
    Hour = 180,

    /// <summary>
    /// День
    /// </summary>
    [Description("День")]
    Day = 200,

    /// <summary>
    /// Неделя
    /// </summary>
    [Description("Неделя")]
    Week = 220,

    /// <summary>
    /// Месяц
    /// </summary>
    [Description("Месяц")]
    Month = 240,

    /// <summary>
    /// None
    /// </summary>
    [Description("-нет-")]
    None = 10000,
}