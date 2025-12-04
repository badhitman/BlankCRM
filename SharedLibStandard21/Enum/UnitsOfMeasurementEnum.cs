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
    /// Литр
    /// </summary>
    [Description("Литр")]
    Liter = 40,

    /// <summary>
    /// Коробка
    /// </summary>
    [Description("Коробка")]
    Box = 60,

    /// <summary>
    /// Кг
    /// </summary>
    [Description("Кг")]
    Kilogram = 80,

    /// <summary>
    /// Бутылка
    /// </summary>
    [Description("Бутылка")]
    Bottle = 100,

    /// <summary>
    /// Кипа
    /// </summary>
    [Description("Кипа")]
    Stack = 120,

    /// <summary>
    /// Связка
    /// </summary>
    [Description("Связка")]
    Bunch = 140,

    /// <summary>
    /// Час
    /// </summary>
    [Description("Час")]
    Hour = 160,

    /// <summary>
    /// День
    /// </summary>
    [Description("День")]
    Day = 180,

    /// <summary>
    /// Неделя
    /// </summary>
    [Description("Неделя")]
    Week = 200,

    /// <summary>
    /// Месяц
    /// </summary>
    [Description("Месяц")]
    Month = 220,

    /// <summary>
    /// None
    /// </summary>
    [Description("-нет-")]
    None = 10000,
}