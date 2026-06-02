////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;

namespace BlazorRetailLib.Components.Payments;

/// <summary>
/// WalletsClientComponent
/// </summary>
public partial class WalletsClientComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string ClientId { get; set; }
}