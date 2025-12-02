////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Commerce.Attendances;
using BlazorLib.Components.Commerce;
using System.ComponentModel;

namespace BlazorLib;

/// <summary>
/// OffersListModesEnum
/// </summary>
public enum OffersListModesEnum
{
    /// <summary>
    /// <see cref="OffersGoodsListComponent"/>
    /// </summary>
    [Description(nameof(OffersGoodsListComponent))]
    Goods,

    /// <summary>
    /// <see cref="OffersAttendancesListComponent"/>
    /// </summary>
    [Description(nameof(OffersAttendancesListComponent))]
    Attendances,
}