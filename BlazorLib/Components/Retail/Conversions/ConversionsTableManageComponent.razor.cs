////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Conversions;

/// <summary>
/// ConversionsTableManageComponent
/// </summary>
public partial class ConversionsTableManageComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public int ExcludeOrderId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public Action<TableRowClickEventArgs<WalletConversionRetailDocumentModelDB>>? RowClickEventHandler { get; set; }

    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }


    bool showDeleted;
    bool ShowDeleted
    {
        get => showDeleted;
        set
        {
            showDeleted = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
        }
    }
    MudTable<WalletConversionRetailDocumentModelDB>? tableRef;

    bool _visible;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true,
    };


    DateRange? _dateRange;
    DateRange? DateRangeProp
    {
        get => _dateRange;
        set
        {
            _dateRange = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
        }
    }

    void CreateNewConversionOpenDialog()
    {
        _visible = true;
    }

    async Task DisabledToggle(int conversionId)
    {
        await SetBusyAsync();

        ResponseBaseModel res = await RetailRepo.DeleteToggleConversionAsync(conversionId);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    async Task<TableData<WalletConversionRetailDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            Payload = new()
            {
                IncludeDisabled = ShowDeleted
            }
        };

        if (DateRangeProp is not null)
        {
            req.Payload.Start = DateRangeProp.Start;
            req.Payload.End = DateRangeProp.End;
        }

        await SetBusyAsync(token: token);
        TPaginationResponseModel<WalletConversionRetailDocumentModelDB> res = await RetailRepo.SelectConversionsDocumentsAsync(req, token);

        await SetBusyAsync(token: token);
        return new TableData<WalletConversionRetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}