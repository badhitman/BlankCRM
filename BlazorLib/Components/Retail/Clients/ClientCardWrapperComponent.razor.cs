////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib.Components.Retail.Clients;

/// <summary>
/// ClientCardWrapperComponent
/// </summary>
public partial class ClientCardWrapperComponent 
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string ClientId { get; set; }
}