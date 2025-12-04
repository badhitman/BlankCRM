////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Payments;

/// <summary>
/// PaymentsManageComponent
/// </summary>
public partial class PaymentsManageComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }


    bool _visible;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true
    };


    void CreateNewOrderOpenDialog()
    {
        _visible = true;
    }

    async Task<TableData<PaymentRetailDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            Payload = new()
            {
                PayerFilterIdentityId = ClientId
            }
        };
        TPaginationResponseModel<PaymentRetailDocumentModelDB>? res = await RetailRepo.SelectPaymentsDocumentsAsync(req, token);
        await SetBusyAsync(false, token: token);

        if (res.Response is not null)
            await CacheUsersUpdate([.. res.Response.Select(x => x.PayerIdentityUserId)]);

        return new TableData<PaymentRetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}