////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// AdapterEditComponent
/// </summary>
public partial class AdapterEditComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IManageStockSharpService SsRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required int Id { get; set; }


    FixMessageAdapterModelDB? originAdapter;
    FixMessageAdapterModelDB? editAdapter;

    bool initDelete;
    AdaptersTypesNames? AdapterType
    {
        get => editAdapter?.AdapterTypeName;
        set
        {
            if (editAdapter is null)
                return;

            if (value is null)
            {
                editAdapter = null;
                return;
            }

            editAdapter.AdapterTypeName = value.Value;
        }
    }

    async Task SaveAdapter()
    {
        if (editAdapter is null)
        {
            SnackbarRepo.Error("adapter is null");
            return;
        }

        await SetBusyAsync();
        TResponseModel<FixMessageAdapterModelDB> res = await SsRepo.UpdateOrCreateAdapterAsync(editAdapter);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success() && res.Response is not null && res.Response.Id > 0)
        {
            originAdapter = res.Response;
            editAdapter = GlobalTools.CreateDeepCopy(originAdapter);
        }
        await SetBusyAsync(false);
    }

    async Task DeleteAdapter()
    {
        if (editAdapter is null)
        {
            SnackbarRepo.Error("adapter is null");
            return;
        }

        if (!initDelete)
        {
            initDelete = true;
            return;
        }

        await SetBusyAsync();
        var res = await SsRepo.DeleteAdapterAsync(editAdapter);
        initDelete = false;
        await SetBusyAsync(false);
        NavRepo.NavigateTo("adapters");
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Id < 1)
        {
            originAdapter = FixMessageAdapterModelDB.BuildEmpty();
            editAdapter = FixMessageAdapterModelDB.BuildEmpty();
            return;
        }

        await SetBusyAsync();
        TResponseModel<FixMessageAdapterModelDB[]> res = await SsRepo.AdaptersGetAsync([Id]);
        SnackbarRepo.ShowMessagesResponse(res.Messages);

        originAdapter = res.Response?.Single();
        editAdapter = GlobalTools.CreateDeepCopy(originAdapter);
        await SetBusyAsync(false);
    }
}