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
    public DocumentRetailModelDB? ExcludeOrder { get; set; }

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
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        await SetBusyAsync();
        TResponseModel<Guid?> res = await RetailRepo.DeleteToggleConversionRetailAsync(new()
        {
            Payload = conversionId,
            SenderActionUserId = CurrentUserSession.UserId
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    void RowClickEvent(TableRowClickEventArgs<WalletConversionRetailDocumentModelDB> tableRowClickEventArgs)
    {
        if (RowClickEventHandler is not null)
            RowClickEventHandler(tableRowClickEventArgs);
    }

    async Task<TableData<WalletConversionRetailDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            SortBy = state.SortLabel,
            Payload = new()
            {
                IncludeDisabled = ShowDeleted,
                RecipientsUserFilter = string.IsNullOrWhiteSpace(ClientId)
                    ? null
                    : [ClientId],
                SendersUserFilter = string.IsNullOrWhiteSpace(ClientId)
                    ? null
                    : [ClientId],
            },
        };

        if (DateRangeProp is not null)
        {
            req.Payload.Start = DateRangeProp.Start;
            req.Payload.End = DateRangeProp.End;
        }

        if (ExcludeOrder is not null && ExcludeOrder.Id > 0)
            req.Payload.ExcludeOrderId = ExcludeOrder.Id;

        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<WalletConversionRetailDocumentModelDB> res = await RetailRepo.SelectConversionsDocumentsRetailAsync(req, token);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);

        if (res.Response is not null && res.Response.Count != 0)
            await CacheUsersUpdate([.. res.Response.Select(x => x.FromWallet!.UserIdentityId).Union(res.Response.Select(x => x.ToWallet!.UserIdentityId))]);

        await SetBusyAsync(token: token);
        return new TableData<WalletConversionRetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}