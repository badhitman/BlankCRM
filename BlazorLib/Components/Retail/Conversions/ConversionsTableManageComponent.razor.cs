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


    bool _visible;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true,         
    };


    void CreateNewConversionOpenDialog()
    {
        _visible = true;
    }

    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }

    async Task<TableData<WalletConversionRetailDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
        };
        await SetBusyAsync(token: token);
        TPaginationResponseModel<WalletConversionRetailDocumentModelDB> res = await RetailRepo.SelectConversionsDocumentsAsync(req, token);

        await SetBusyAsync(token: token);
        return new TableData<WalletConversionRetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}