////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib;

/// <summary>
/// OrderLinkBaseComponent
/// </summary>
public class OrderLinkBaseComponent<T> : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    protected IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public int OrderId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public Action? ReloadOrdersLinks { get; set; }

    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }


    /// <inheritdoc/>
    protected bool
        _visibleIncludeOrder,
        _visibleCreateNewOrder;


    /// <inheritdoc/>
    protected MudTable<T>? tableRef;

    /// <inheritdoc/>
    protected T? elementBeforeEdit;


    /// <inheritdoc/>
    protected readonly static DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true,
    };

    /// <inheritdoc/>
    protected int initDeleteRow;


    /// <inheritdoc/>
    protected void IncludeExistOrderOpenDialog() => _visibleIncludeOrder = true;

    /// <inheritdoc/>
    protected async Task ItemHasBeenCommittedFinalize()
    {
        initDeleteRow = 0;

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
        if (ReloadOrdersLinks is not null)
            ReloadOrdersLinks();
    }

    /// <inheritdoc/>
    protected async Task DeleteRowFinalize()
    {
        initDeleteRow = 0;
        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);

        if (ReloadOrdersLinks is not null)
            ReloadOrdersLinks();
    }

    /// <inheritdoc/>
    protected void BackupItem(object element)
    {
        initDeleteRow = 0;
        if (element is T other)
            elementBeforeEdit = GlobalTools.CreateDeepCopy(other)!;
    }

    /// <inheritdoc/>
    protected bool CanDeleteRow(int rowLinkId)
    {
        if (initDeleteRow == 0)
        {
            initDeleteRow = rowLinkId;
            return false;
        }
        if (initDeleteRow != rowLinkId)
        {
            initDeleteRow = 0;
            return false;
        }

        return true;
    }
}