////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Specifies the possible connection states.
/// </summary>
public enum ConnectionStatesEnum
{
    /// <summary>
    /// The connection is currently disconnected.
    /// </summary>
    [Description("The connection is currently disconnected.")]
    Disconnected,

    /// <summary>
    /// The connection is in the process of disconnecting.
    /// </summary>
    [Description("The connection is in the process of disconnecting.")]
    Disconnecting,

    /// <summary>
    /// The connection is in the process of connecting.
    /// </summary>
    [Description("The connection is in the process of connecting.")]
    Connecting,

    /// <summary>
    /// The connection is successfully established.
    /// </summary>
    [Description("The connection is successfully established.")]
    Connected,

    /// <summary>
    /// The connection is attempting to reconnect.
    /// </summary>
    [Description("The connection is attempting to reconnect.")]
    Reconnecting,

    /// <summary>
    /// The connection has been restored.
    /// </summary>
    [Description("The connection has been restored.")]
    Restored,

    /// <summary>
    /// The connection attempt has failed.
    /// </summary>
    [Description("The connection attempt has failed.")]
    Failed
}