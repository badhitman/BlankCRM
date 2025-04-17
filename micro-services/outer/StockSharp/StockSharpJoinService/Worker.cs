using NetMQ;
using NetMQ.Sockets;

namespace StockSharpJoinService;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly ResponseSocket server = new("@tcp://localhost:5557");

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            string fromClientMessage = server.ReceiveFrameString();
            Console.WriteLine("From Client: {0}", fromClientMessage);
            server.SendFrame("World");
        }
        server.Dispose();
        return Task.CompletedTask;
    }
}
