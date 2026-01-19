////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components;

/// <summary>
/// HistoryViewBaseComponent
/// </summary>
public abstract class HistoryViewBaseComponent : BlazorBusyComponentBaseAuthModel
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required HistoryOwnerBaseComponent OwnerBase { get; set; }


    /// <inheritdoc/>
    [Inject]
    IJSRuntime JS { get; set; } = default!;


    /// <inheritdoc/>
    protected MudTable<TraceReceiverRecord>? tableRef;

    DateRange? _dateRangePeriod;
    /// <inheritdoc/>
    protected DateRange? DateRangePeriod
    {
        get => _dateRangePeriod;
        set
        {
            _dateRangePeriod = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
        }
    }


    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        await JS.InvokeVoidAsync("HighlightBlock.Init");
    }

    /// <inheritdoc/>
    protected async Task<TableData<TraceReceiverRecord>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TableData<TraceReceiverRecord> res = await OwnerBase.ServerReload(DateRangePeriod, state, token);

        IEnumerable<string?>? usersIds = res.Items?.Select(x => x.SenderActionUserId);
        if (usersIds is not null)
            await OwnerBase.CacheUsersUpdate([.. usersIds.Where(x => !string.IsNullOrWhiteSpace(x))!]);

        await SetBusyAsync(false, token);
        return res;
    }
}