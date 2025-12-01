////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveryManageComponent
/// </summary>
public partial class DeliveryManageComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Parameter]
    public string? FilterClientId { get; set; }
}