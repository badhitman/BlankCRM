////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib.Components.Retail.Clients;

/// <summary>
/// OrdersListComponent
/// </summary>
public partial class OrdersListComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Parameter]
    public string? FilterClientId { get; set; }
}