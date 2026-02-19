////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// TraceNetMQConfig
/// </summary>
public class TraceNetMQConfigModel : HostConfigModel
{
    /// <inheritdoc/>
    public static TraceNetMQConfigModel BuildEmpty()
    {
        return new TraceNetMQConfigModel() { Scheme = "tcp", Port = 2883 };
    }

    /// <inheritdoc/>
    public List<string>? TracesNamesPatterns { get; set; }

    /// <inheritdoc/>
    public void Reload(TraceNetMQConfigModel other)
    {
        Scheme = other.Scheme;
        Host = other.Host;
        Port = other.Port;
    }

    /// <inheritdoc/>
    public static readonly string Configuration = "TraceNetMQConfig";
}