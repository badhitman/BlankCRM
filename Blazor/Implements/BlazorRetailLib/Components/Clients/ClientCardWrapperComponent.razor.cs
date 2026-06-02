////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorRetailLib.Components.Clients;

/// <summary>
/// ClientCardWrapperComponent
/// </summary>
public partial class ClientCardWrapperComponent 
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string ClientId { get; set; }
}