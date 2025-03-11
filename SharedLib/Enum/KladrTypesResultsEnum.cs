////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// KladrTypesResultsEnum
/// </summary>
public enum KladrTypesResultsEnum
{
    /// <summary>
    /// 0 регионы
    /// </summary>
    RootRegions = 0,

    #region Регион
    /// <summary>
    /// 1.1 города в регионе
    /// </summary>
    CitiesInRegion = 10,

    /// <summary>
    /// 1.2 нас. пункты в регионе
    /// </summary>
    PopPointsInRegion = 20,

    /// <summary>
    /// 1.3 районы в регионе
    /// </summary>
    AreasInRegion = 30,

    /// <summary>
    /// 1.4 street`s в регионе
    /// </summary>
    StreetsInRegion = 40,
    #endregion

    #region Район
    /// <summary>
    /// 2.1 города в районах
    /// </summary>
    CitiesInArea =50,

    /// <summary>
    /// 2.2 нас. пункты в районах
    /// </summary>
    PopPointsInArea = 60,
    #endregion

    #region Город
    /// <summary>
    /// 3.1 нас. пункты в городах
    /// </summary>
    PopPointsInCity = 70,

    /// <summary>
    /// 3.2 улицы в городах
    /// </summary>
    StreetsInCity = 80,
    #endregion

    #region Нас.пункт
    /// <summary>
    /// 4.1 улицы в городах StreetKLADRModelDB
    /// </summary>
    StreetsInPopPoint = 90,
    #endregion

    #region Улица
    /// <summary>
    /// 5.1 дома на улицах
    /// </summary>
    HousesInStrin = 100,
    #endregion
}