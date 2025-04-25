////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace StockSharpDriver;

/// <inheritdoc/>
public class ConnectionStockSharpWorker(
    ILogger<ConnectionStockSharpWorker> logger,
    StockSharpClientConfigModel conf,
    StockSharp.Algo.Connector _connector) : BackgroundService
{
    /// <inheritdoc/>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

        }

        if (!conf.CancelOrdersWithStop)
            _connector.CancelOrders();

        return Task.CompletedTask;
    }
}