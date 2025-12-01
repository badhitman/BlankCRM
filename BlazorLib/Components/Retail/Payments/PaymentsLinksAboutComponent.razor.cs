////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Payments;

/// <summary>
/// PaymentsLinksAboutComponent
/// </summary>
public partial class PaymentsLinksAboutComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public List<PaymentRetailOrderLinkModelDB> PaymentOrdersLinks { get; set; }

    /// <summary>
    /// Сумма документа
    /// </summary>
    [Parameter]
    public decimal? Amount { get; set; }
}