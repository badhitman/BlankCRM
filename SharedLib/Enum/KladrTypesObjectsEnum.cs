////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Типы объектов
/// </summary>
public enum KladrTypesObjectsEnum
{
    /// <summary>
    /// Регион
    /// </summary>
    [Description("Регион")]
    RootRegion = 10,

    /// <summary>
    /// Район
    /// </summary>
    [Description("Район")]
    Area = 20,

    /// <summary>
    /// Город
    /// </summary>
    [Description("Город")]
    City = 30,

    /// <summary>
    /// Нас. пункт
    /// </summary>
    [Description("Населённый пункт")]
    PopPoint = 40,

    /// <summary>
    /// Улица
    /// </summary>
    [Description("Улица")]
    Street = 50,

    /// <summary>
    /// Дом
    /// </summary>
    [Description("Дом")]
    House = 60,
}
