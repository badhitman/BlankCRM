////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using System;

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
    public Task RegisterAction(string QueueName, Action<T> payload, CancellationToken stoppingToken = default);

    /// <summary>
    /// UnregisterAction
    /// </summary>
    public Task UnregisterAction(CancellationToken stoppingToken = default);
}