////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib.Components.Retail.Clients;

public partial class ClientCardWrapperComponent 
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string ClientId { get; set; }
}