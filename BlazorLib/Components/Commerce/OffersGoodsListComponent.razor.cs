////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// OffersGoodsListComponent
/// </summary>
public partial class OffersGoodsListComponent : BlazorRegistersComponent
{
    [Inject]
    IParametersStorageTransmission StorageTransmissionRepo { get; set; } = default!;


    /// <summary>
    /// CurrentNomenclature
    /// </summary>
    [Parameter, EditorRequired]
    public required NomenclatureModelDB CurrentNomenclature { get; set; }


    bool
        _hideMultiplicity,
        _hideWorth,
        loadTableReady;

    private MudTable<OfferModelDB> table = default!;
    bool _visibleChangeConfig;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        CloseButton = true,
        CloseOnEscapeKey = true,
    };

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();

        List<Task> tasks = [
            Task.Run(async () => { TResponseModel<bool?> res = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.HideWorthOffers); if (!res.Success()) SnackBarRepo.ShowMessagesResponse(res.Messages); else _hideWorth = res.Response == true; }),
            Task.Run(async () => { TResponseModel<bool?> res = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.HideMultiplicityOffers); if (!res.Success()) SnackBarRepo.ShowMessagesResponse(res.Messages); else _hideMultiplicity = res.Response == true;})];

        await Task.WhenAll(tasks);
        loadTableReady = true;
        await SetBusyAsync(false);

        if (table is not null)
            await table.ReloadServerData();
    }

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
        if (!loadTableReady || CurrentUserSession is null)
            return new TableData<OfferModelDB>() { TotalItems = 0, Items = [] };

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

        if (res.Response?.Response is not null)
        {
            await CacheRegistersUpdate(offers: res.Response.Response.Select(x => x.Id).ToArray(), goods: []);
            await SetBusyAsync(false, token);
            return new TableData<OfferModelDB>() { TotalItems = res.Response.TotalRowsCount, Items = res.Response.Response };
        }

        await SetBusyAsync(false, token);
        return new TableData<OfferModelDB>() { TotalItems = 0, Items = [] };
    }

    bool _expanded;
    private void OnExpandCollapseClick()
    {
        _expanded = !_expanded;
    }
}