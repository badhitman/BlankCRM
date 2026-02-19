////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using SharedLib;

namespace RealtimeService;

/// <summary>
/// NetMqBackgroundService
/// </summary>
public class NetMqBackgroundService(IOptions<TraceNetMQConfigModel> _conf) : BackgroundService
{
    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        using XPublisherSocket xpubSocket = new();
        using XSubscriberSocket xsubSocket = new();
        xpubSocket.Bind("tcp://127.0.0.1:1234");
        xsubSocket.Bind("tcp://127.0.0.1:5678");
        Console.WriteLine("Intermediary started, and waiting for messages");
        // proxy messages between frontend / backend
        var proxy = new Proxy(xsubSocket, xpubSocket);
        // blocks indefinitely
        proxy.Start();
    }
}