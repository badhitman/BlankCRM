////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib.Components.Retail.Payments;

/// <summary>
/// WalletsClientComponent
/// </summary>
public partial class WalletsClientComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public string ClientId { get; set; }
}