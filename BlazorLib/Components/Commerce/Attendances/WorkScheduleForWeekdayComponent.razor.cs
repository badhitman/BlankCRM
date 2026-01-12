////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Commerce.Attendances;

/// <summary>
/// WorkScheduleForWeekdayComponent
/// </summary>
public partial class WorkScheduleForWeekdayComponent : BlazorBusyComponentBaseAuthModel
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
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        await SetBusyAsync();
        TResponseModel<int> res = await CommerceRepo.WeeklyScheduleCreateOrUpdateAsync(new()
        {
            Payload = WorkScheduleEdit,
            SenderActionUserId = CurrentUserSession.UserId
        });

        if (res.Success())
        {
            WorkScheduleEdit.LastUpdatedAtUTC = DateTime.UtcNow;
            WorkSchedule = GlobalTools.CreateDeepCopy(WorkScheduleEdit)!;
        }

        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        WorkScheduleEdit = GlobalTools.CreateDeepCopy(WorkSchedule)!;
        await base.OnInitializedAsync();
    }
}