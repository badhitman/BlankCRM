////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System;
using System.Collections;
using System.Net;

namespace SharedLib;

/// <summary>
/// MqttClientModel
/// </summary>
public class MqttClientModel
{
    /// <inheritdoc/>
    public long BytesReceived { get; set; }

    /// <inheritdoc/>
    public long BytesSent { get; set; }

    /// <inheritdoc/>
    public DateTime ConnectedTimestamp { get; set; }

    /// <inheritdoc/>
    public string? RemoteEndPoint { get; set; }

    /// <inheritdoc/>
    public string? Id { get; set; }

    /// <inheritdoc/>
    public DateTime LastNonKeepAlivePacketReceivedTimestamp { get; set; }

    /// <inheritdoc/>
    public DateTime LastPacketReceivedTimestamp { get; set; }

    /// <inheritdoc/>
    public DateTime LastPacketSentTimestamp { get; set; }

    /// <inheritdoc/>
    public string? ProtocolVersion { get; set; }

    /// <inheritdoc/>
    public long ReceivedApplicationMessagesCount { get; set; }

    /// <inheritdoc/>
    public long ReceivedPacketsCount { get; set; }

    /// <inheritdoc/>
    public long SentApplicationMessagesCount { get; set; }

    /// <inheritdoc/>
    public long SentPacketsCount { get; set; }
}