////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Payments;

/// <summary>
/// PaymentDocumentComponent
/// </summary>
public partial class PaymentDocumentComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    //[Inject]
    //IRubricsTransmission HelpDeskRepo { get; set; } = default!;

    //[Inject]
    //IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public int PaymentId { get; set; }

    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }


    PaymentRetailDocumentModelDB? currentDoc, editDoc;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (PaymentId < 1)
        {
            currentDoc = new()
            {
                DatePayment = DateTime.UtcNow,
                StatusPayment = PaymentsRetailStatusesEnum.Awaiting,
            };
            editDoc = GlobalTools.CreateDeepCopy(currentDoc);
        }
        else
        {
            //await resDoc = await RetailRepo.pay();
        }
    }

    void SelectWalletRecipientAction(WalletRetailModelDB? wallet)
    {
        //if (editDoc is null)
        //    throw new ArgumentNullException(nameof(editDoc));

        //editDoc.ToWallet = wallet;
        //editDoc.ToWalletId = wallet?.Id ?? 0;
        //InvokeAsync(UpdateUsers);
        StateHasChanged();
    }


    void SelectUserRecipientAction(UserInfoModel? user)
    {
        //userRecipient = user;
        StateHasChanged();
    }
}