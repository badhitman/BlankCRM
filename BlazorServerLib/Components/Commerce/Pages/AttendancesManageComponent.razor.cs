﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorWebLib.Components.Commerce.Attendances;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Commerce.Pages;

/// <summary>
/// AttendancesManageComponent
/// </summary>
public partial class AttendancesManageComponent : BlazorBusyComponentBaseAuthModel
{
    /// <summary>
    /// Commerce
    /// </summary>
    [Inject]
    protected ICommerceRemoteTransmissionService CommerceRepo { get; set; } = default!;


    OfferModelDB? SelectedOffer { get; set; }
    int? _selectedOfferId;
    /// <summary>
    /// SelectedOfferId
    /// </summary>
    public int? SelectedOfferId
    {
        get => _selectedOfferId;
        set
        {
            _selectedOfferId = value;
            SelectedOffer = AllOffers.First(x => x.Id == value);

            if (_workSchedule is not null)
                InvokeAsync(async () => await _workSchedule.Reload(SelectedOffer));
            if (_workCalendar is not null)
                InvokeAsync(async () => await _workCalendar.Reload(SelectedOffer));
            if (workOrganizations is not null)
                InvokeAsync(async () => await workOrganizations.Reload(SelectedOffer));
        }
    }
    List<OfferModelDB> AllOffers { get; set; } = [];
    IGrouping<NomenclatureModelDB?, OfferModelDB>[] OffersNodes => AllOffers.GroupBy(x => x.Nomenclature).ToArray();

    WorkScheduleComponent? _workSchedule;
    WorkCalendarComponent? _workCalendar;
    WorksSchedulersOrganizationsComponent? workOrganizations;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await Task.WhenAll([LoadOffers(0), ReadCurrentUser()]);
    }

    /// <summary>
    /// LoadOffers
    /// </summary>
    protected async Task LoadOffers(int page_num)
    {
        if (page_num == 0)
            AllOffers = [new() { Name = "Глобально", Nomenclature = new() { Name = "Стандарт" } }];

        TPaginationRequestModel<OffersSelectRequestModel> req = new()
        {
            PageNum = page_num,
            PageSize = 10,
            SortBy = nameof(OfferModelDB.Name),
            SortingDirection = VerticalDirectionsEnum.Up,
            Payload = new()
            {
                ContextName = GlobalStaticConstants.Routes.ATTENDANCES_CONTROLLER_NAME
            }
        };
        await SetBusy();

        TPaginationResponseModel<OfferModelDB> res = await CommerceRepo.OffersSelect(req);
        await SetBusy(false);
        if (res.Response is not null && res.Response.Count != 0)
        {
            AllOffers!.AddRange(res.Response);
            if (AllOffers.Count < res.TotalRowsCount)
                await LoadOffers(page_num + 1);
        }
    }
}