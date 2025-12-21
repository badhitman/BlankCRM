////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Orders;

public partial class OrderPaymentsAboutComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required List<PaymentOrderRetailLinkModelDB> PaymentsDocuments { get; set; }

    PaymentOrderRetailLinkModelDB[]? ActualConversionsDocuments;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ActualConversionsDocuments = [.. PaymentsDocuments.Where(x => x.PaymentDocument?.StatusPayment == PaymentsRetailStatusesEnum.Paid)];
    }
}