////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorCommerceLib.Components.Attendances;
using BlazorCommerceLib.Components;
using System.ComponentModel;
using BlazorCommerceLib.Components.Offers;

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