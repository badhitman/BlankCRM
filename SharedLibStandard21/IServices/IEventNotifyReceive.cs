////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// Уведомления о событиях (server/driver -> client/ui)
/// </summary>
public interface IEventNotifyReceive<T>
{
    /// <summary>
    /// AccountHandler
    /// </summary>
    public delegate void AccountHandler(T message);

    /// <summary>
    /// Notify
    /// </summary>
    public event AccountHandler Notify;

    /// <summary>
    /// RegisterAction
    /// </summary>
    public Task RegisterAction(string QueueName, Action<T> payload, string layoutContainerId, byte[]? userInfoBytes, bool isMute = false, List<KeyValuePair<string, byte[]>>? propertiesValues = null, CancellationToken stoppingToken = default);

    /// <summary>
    /// UnregisterAction
    /// </summary>
    public Task UnregisterAction(bool isMute = false, CancellationToken stoppingToken = default);
}