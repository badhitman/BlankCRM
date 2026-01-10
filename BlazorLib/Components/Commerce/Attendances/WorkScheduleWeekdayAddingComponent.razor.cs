////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Commerce.Attendances;

/// <summary>
/// Добавление/Создание WorkSchedule для Weekday
/// </summary>
public partial class WorkScheduleWeekdayAddingComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// Weekday
    /// </summary>
    [Parameter, EditorRequired]
    public required DayOfWeek Weekday { get; set; }

    /// <summary>
    /// Создание/добавление нового WorkSchedule
    /// </summary>
    [Parameter]
    public required Action<WeeklyScheduleModelDB>? AddingWorkScheduleHandle { get; set; }

    /// <summary>
    /// Offer
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required OfferModelDB? Offer { get; set; }



    bool CantSave => EndPart is null || StartPart is null || EndPart < StartPart;

    bool IsExpandAdding;

    /// <summary>
    /// StartPart
    /// </summary>
    TimeSpan? StartPart = new(09, 00, 00);

    /// <summary>
    /// EndPart
    /// </summary>
    TimeSpan? EndPart = new(18, 00, 00);

    uint QueueCapacity;

    async Task Save()
    {
        if (EndPart is null || StartPart is null)
            return;

        WeeklyScheduleModelDB ws = new()
        {
            Name = "",
            EndPart = EndPart.Value,
            StartPart = StartPart.Value,
            Weekday = Weekday,
            QueueCapacity = QueueCapacity,
            ContextName = Routes.ATTENDANCES_CONTROLLER_NAME,
            IsDisabled = true, 
            OfferId = Offer?.Id
        };

        await SetBusyAsync();
        TResponseModel<int> res = await CommerceRepo.WeeklyScheduleCreateOrUpdateAsync(ws);
        ws.Id = res.Response;
        if (res.Success() && ws.Id != 0 && AddingWorkScheduleHandle is not null)
        {
            IsExpandAdding = !IsExpandAdding;
            AddingWorkScheduleHandle(ws);
        }
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
    }
}