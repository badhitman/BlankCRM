////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Commerce.Attendances;

/// <summary>
/// AttendancesCatalogComponent
/// </summary>
public partial class AttendancesCatalogComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    bool _expanded;
    MudTable<NomenclatureModelDB> tableRef = default!;
    List<RecordsAttendanceModelDB> currentRecords = [];

    async void CreateNomenclatureAction(NomenclatureModelDB nom)
    {
        await tableRef.ReloadServerData();
        OnExpandCollapseClick();
        StateHasChanged();
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<NomenclatureModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestModel<NomenclaturesSelectRequestModel> req = new()
        {
            Payload = new() { ContextName = GlobalStaticConstants.Routes.ATTENDANCES_CONTROLLER_NAME },
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
        };
        await SetBusyAsync(token: token);
        TPaginationResponseModel<NomenclatureModelDB> resNomenclatures = await CommerceRepo.NomenclaturesSelectAsync(req, token);

        IsBusyProgress = false;

        if (resNomenclatures.Response is null)
            return new TableData<NomenclatureModelDB>() { TotalItems = 0, Items = [] };


        TPaginationRequestAuthModel<RecordsAttendancesRequestModel> recReq = new()
        {
            Payload = new RecordsAttendancesRequestModel()
            {
                NomenclatureFilter = [.. resNomenclatures.Response.Select(x => x.Id)],
                ContextName = GlobalStaticConstants.Routes.ATTENDANCES_CONTROLLER_NAME,
                IncludeExternalData = true,
            },
            SenderActionUserId = CurrentUserSession!.UserId,
            PageNum = 0,
            PageSize = int.MaxValue,
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
        };

        TPaginationResponseModel<RecordsAttendanceModelDB> recordsSelect = await CommerceRepo.RecordsAttendancesSelectAsync(recReq, token);
        List<RecordsAttendanceModelDB> currentRecords = recordsSelect.Response ?? [];

        return new TableData<NomenclatureModelDB>() { TotalItems = resNomenclatures.TotalRowsCount, Items = resNomenclatures.Response };
    }

    private void OnExpandCollapseClick()
    {
        _expanded = !_expanded;
    }
}