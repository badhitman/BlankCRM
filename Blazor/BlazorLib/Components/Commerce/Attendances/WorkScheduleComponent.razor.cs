////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib;
using SharedLib;

namespace BlazorLib.Components.Commerce.Attendances;

/// <summary>
/// WorkScheduleComponent
/// </summary>
public partial class WorkScheduleComponent : BlazorBusyComponentBaseAuthModel
{
    WorkSchedulesOfWeekdayComponent? WorkSchedulesOfWeekday_ref;

    /// <summary>
    /// Reload OfferModelDB selectedOffer
    /// </summary>
    public async Task Reload(OfferModelDB? selectedOffer)
    {
        await SetBusyAsync();
        if (WorkSchedulesOfWeekday_ref is not null)
            await WorkSchedulesOfWeekday_ref.LoadData(0, selectedOffer);
        await SetBusyAsync(false);
    }
}