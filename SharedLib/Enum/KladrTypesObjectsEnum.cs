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
    RootRegion = 0,

    /// <summary>
    /// Район
    /// </summary>
    [Description("Район")]
    Area = 10,

    /// <summary>
    /// Город
    /// </summary>
    [Description("Город")]
    City = 20,

    /// <summary>
    /// Нас. пункт
    /// </summary>
    [Description("Населённый пункт")]
    PopPoint = 30,

    /// <summary>
    /// Улица
    /// </summary>
    [Description("Улица")]
    Street = 40,
    
    /// <summary>
    /// Дом
    /// </summary>
    [Description("Дом")]
    Home = 50,
}
