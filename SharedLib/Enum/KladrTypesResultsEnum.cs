////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Тип подчинения вышестоящему объекту
/// </summary>
public enum KladrTypesResultsEnum
{
    /// <summary>
    /// 0 [ни кому не подчинено] регионы
    /// </summary>
    [Description("root")]
    RootRegions = 0,

    #region Регион
    /// <summary>
    /// 1.1 города в регионе
    /// </summary>
    [Description("регион->город")]
    CitiesInRegion = 10,

    /// <summary>
    /// 1.2 нас. пункты в регионе
    /// </summary>
    [Description("регион->нас.пункт")]
    PopPointsInRegion = 20,

    /// <summary>
    /// 1.3 районы в регионе
    /// </summary>
    [Description("регион->район")]
    AreasInRegion = 30,

    /// <summary>
    /// 1.4 street`s в регионе
    /// </summary>
    [Description("регион->улица")]
    StreetsInRegion = 40,
    #endregion

    #region Район
    /// <summary>
    /// 2.1 города в районах
    /// </summary>
    [Description("район->город")]
    CitiesInArea = 50,

    /// <summary>
    /// 2.2 нас. пункты в районах
    /// </summary>
    [Description("район->нас.пункт")]
    PopPointsInArea = 60,
    #endregion

    #region Город
    /// <summary>
    /// 3.1 нас. пункты в городах
    /// </summary>
    [Description("город->нас.пункт")]
    PopPointsInCity = 70,

    /// <summary>
    /// 3.2 улицы в городах
    /// </summary>
    [Description("город->улица")]
    StreetsInCity = 80,
    #endregion

    #region Нас.пункт
    /// <summary>
    /// 4.1 улицы в нас.пунктах
    /// </summary>
    [Description("нас.пункт->улица")]
    StreetsInPopPoint = 90,
    #endregion

    #region Улица
    /// <summary>
    /// 5.1 дома на улицах
    /// </summary>
    [Description("улица->дом")]
    HousesInStreet = 100,
    #endregion
}