////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using MQTTnet.Diagnostics;
using MQTTnet.Server;
using SharedLib;
using MQTTnet;

namespace StockSharpDriver;

/// <inheritdoc/>
public class MqttServerWorker : BackgroundService
{
    //private const string _connectorFile = "ConnectorFile.json";
    readonly ILogger<MqttServerWorker> _logger;
    readonly MqttServer server;

    /// <inheritdoc/>
    public MqttServerWorker(ILogger<MqttServerWorker> logger, StockSharpClientConfigModel conf)
    {
        _logger = logger;
        MqttFactory mqttFactory = new();
        MqttServerOptions mqttServerOptions = mqttFactory
            .CreateServerOptionsBuilder()
            .WithDefaultEndpoint()
            .WithDefaultEndpointPort(conf.Port)
            .Build();

        server = mqttFactory.CreateMqttServer(mqttServerOptions, new CustomLogger(logger));
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await server.StartAsync();
    }

    class CustomLogger(ILogger<MqttServerWorker> logger) : IMqttNetLogger
    {
        public bool IsEnabled => true;

        public void Publish(MqttNetLogLevel logLevel, string source, string message, object[] parameters, Exception exception)
        {
            if (parameters?.Length > 0)
                message = string.Format(message, parameters);

            switch (logLevel)
            {
                case MqttNetLogLevel.Verbose:
                    logger.LogDebug(message);
                    break;

                case MqttNetLogLevel.Info:
                    logger.LogInformation(message);
                    break;

                case MqttNetLogLevel.Warning:
                    logger.LogWarning(message);
                    break;

                case MqttNetLogLevel.Error:
                    logger.LogError(exception, message);
                    break;
            }
        }
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        server.Dispose();
        base.Dispose();
    }
}