////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Options;
using NetMQ.Sockets;
using SharedLib;
using NetMQ;

namespace RealtimeService;

/// <summary>
/// ProxyBackgroundServiceNetMQ
/// </summary>
public class ProxyBackgroundServiceNetMQ(IOptions<ProxyNetMQConfigModel> _conf) : BackgroundService
{
    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using XPublisherSocket xpubSocket = new();
        using XSubscriberSocket xsubSocket = new();
        xpubSocket.Bind(_conf.Value.PublisherSocketEndpoint.ToString());
        xsubSocket.Bind(_conf.Value.SubscriberSocketEndpoint.ToString());
        //Console.WriteLine("Intermediary started, and waiting for messages");
        // proxy messages between frontend / backend
        Proxy proxy = new(xsubSocket, xpubSocket);
        // blocks indefinitely
        proxy.Start();
    }
}