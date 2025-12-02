////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorLib.Components.Commerce.Attendances;

/// <summary>
/// OffersAttendancesListComponent
/// </summary>
public partial class OffersAttendancesListComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IStorageTransmission StorageTransmissionRepo { get; set; } = default!;

    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// CurrentNomenclature
    /// </summary>
    [Parameter, EditorRequired]
    public required NomenclatureModelDB CurrentNomenclature { get; set; }


    List<RecordsAttendanceModelDB> currentRecords = [];
    private MudTable<OfferModelDB> table = default!;
    bool _visibleChangeConfig;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        CloseButton = true
    };

    void CancelChangeConfig()
    {
        _visibleChangeConfig = !_visibleChangeConfig;
    }

    async void CreateOfferAction(OfferModelDB sender)
    {
        await table.ReloadServerData();
        OnExpandCollapseClick();
        StateHasChanged();
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<OfferModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        if(CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        TPaginationRequestStandardModel<OffersSelectRequestModel> req = new()
        {
            Payload = new()
            {
                NomenclatureFilter = [CurrentNomenclature.Id],
                ContextName = CurrentNomenclature.ContextName,
            },
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };
        await SetBusyAsync(token: token);
        TResponseModel<TPaginationResponseModel<OfferModelDB>> res = await CommerceRepo.OffersSelectAsync(new() { Payload = req, SenderActionUserId = CurrentUserSession.UserId }, token);
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (res.Response?.Response is not null)
        {
            TPaginationRequestAuthModel<RecordsAttendancesRequestModel> recReq = new()
            {
                Payload = new RecordsAttendancesRequestModel()
                {
                    NomenclatureFilter = [CurrentNomenclature.Id],
                    OfferFilter = [.. res.Response.Response.Select(x => x.Id).Distinct()],
                    ContextName = CurrentNomenclature.ContextName,
                    IncludeExternalData = true,
                },
                SenderActionUserId = CurrentUserSession.UserId,
                PageNum = 0,
                PageSize = 100,
                SortingDirection = state.SortDirection.Convert(),
            };

            TPaginationResponseModel<RecordsAttendanceModelDB> recordsSelect = await CommerceRepo.RecordsAttendancesSelectAsync(recReq, token);

            if (recordsSelect.TotalRowsCount > recReq.PageSize)
                SnackBarRepo.Error($"Записей больше: {recordsSelect.TotalRowsCount}");

            currentRecords = recordsSelect.Response ?? [];
            IsBusyProgress = false;
            return new TableData<OfferModelDB>() { TotalItems = res.Response.TotalRowsCount, Items = res.Response.Response };
        }

        IsBusyProgress = false;
        return new TableData<OfferModelDB>() { TotalItems = 0, Items = [] };
    }

    bool _expanded;
    private void OnExpandCollapseClick()
    {
        _expanded = !_expanded;
    }
}