////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Commerce.Attendances;

/// <summary>
/// WorkScheduleForWeekdayComponent
/// </summary>
public partial class WorkScheduleForWeekdayComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// Commerce
    /// </summary>
    [Inject]
    protected ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// WorkSchedule
    /// </summary>
    [Parameter, EditorRequired]
    public required WeeklyScheduleModelDB WorkSchedule { get; set; }

    WeeklyScheduleModelDB WorkScheduleEdit { get; set; } = default!;


    bool IsEdited => WorkSchedule.IsDisabled != WorkScheduleEdit.IsDisabled ||
        WorkSchedule.StartPart != WorkScheduleEdit.StartPart ||
        WorkSchedule.EndPart != WorkScheduleEdit.EndPart ||
        WorkSchedule.QueueCapacity != WorkScheduleEdit.QueueCapacity ||
        WorkSchedule.Name != WorkScheduleEdit.Name ||
        WorkSchedule.Description != WorkScheduleEdit.Description;


    async Task SaveSchedule()
    {
        await SetBusyAsync();
        TResponseModel<int> res = await CommerceRepo.WeeklyScheduleUpdateAsync(WorkScheduleEdit);
        if (res.Success())
        {
            WorkScheduleEdit.LastUpdatedAtUTC = DateTime.UtcNow;
            WorkSchedule = GlobalTools.CreateDeepCopy(WorkScheduleEdit)!;
        }

        await SetBusyAsync(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        WorkScheduleEdit = GlobalTools.CreateDeepCopy(WorkSchedule)!;
    }
}