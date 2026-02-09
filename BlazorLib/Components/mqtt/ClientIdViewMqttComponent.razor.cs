////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib.Components.mqtt;

/// <summary>
/// ClientIdViewMqttComponent
/// </summary>
public partial class ClientIdViewMqttComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string ClientId { get; set; }

    string[]? segments;
    bool showId;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        segments = ClientId.Split(' ');
        if (segments.Length != 3)
            segments = null;

        base.OnInitialized();
    }
}