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
}