////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using SharedLib;
using MQTTnet;
using System;

namespace RemoteCallLib;

/// <summary>
/// EventNotifyReceive
/// </summary>
public class EventNotifyReceive<T> : IEventNotifyReceive<T>
{
    /// <summary>
    /// Notify
    /// </summary>
    public event IEventNotifyReceive<T>.AccountHandler? Notify;

    IMqttClient mqttClient;
    MqttClientFactory mqttFactory = new();

    readonly StockSharpClientConfigModel MQConfigRepo;
    readonly ILogger<EventNotifyReceive<T>> LoggerRepo;

    /// <summary>
    /// EventNotifyReceive
    /// </summary>
    public EventNotifyReceive(
        StockSharpClientConfigModel rabbitConf,
        IServiceProvider servicesProvider,
        ILogger<EventNotifyReceive<T>> loggerRepo)
    {
        LoggerRepo = loggerRepo;
        MQConfigRepo = rabbitConf;

        using IServiceScope scope = servicesProvider.CreateScope();

        mqttClient = mqttFactory.CreateMqttClient();
        //loggerRepo.LogInformation($"Subscriber [{QueueName}] socket ready...");
    }

    /// <inheritdoc/>
    public async Task RegisterAction(string QueueName, Action<T> actNotyfy, CancellationToken stoppingToken = default)
    {
        Task ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            string content = Encoding.UTF8.GetString(e.ApplicationMessage.Payload).Trim();
            T? sr = default;
            try
            {
                sr = content.Equals("null", StringComparison.OrdinalIgnoreCase)
                ? default
                : JsonConvert.DeserializeObject<T?>(content);
            }
            catch (Exception ex)
            {
                LoggerRepo.LogError(ex, $"Ошибка выполнения удалённой команды: {QueueName}");
                //
                e.ReasonCode = MqttApplicationMessageReceivedReasonCode.UnspecifiedError;
                e.ResponseReasonString = ex.Message;
            }
            if (sr is null)
                LoggerRepo.LogError($"Ошибка обработки удалённой команды (source is null): {QueueName}");
            else
            {
                actNotyfy(sr);
                if (Notify is not null)
                    Notify(sr);
            }
            return Task.CompletedTask;
        }

        mqttClient.ApplicationMessageReceivedAsync += ApplicationMessageReceived;

        try
        {
            await mqttClient.ConnectAsync(GetMqttClientOptionsBuilder, stoppingToken);
            await mqttClient.SubscribeAsync(QueueName, cancellationToken: stoppingToken);
        }
        catch (Exception ex)
        {
            LoggerRepo.LogError(ex, "can`t connect/subscribe");
            mqttClient.ApplicationMessageReceivedAsync -= ApplicationMessageReceived;
            return;
        }
    }

    /// <inheritdoc/>
    public Task UnregisterAction(CancellationToken stoppingToken = default)
    {
        mqttClient.Dispose();
        Notify = null;
        return Task.CompletedTask;
    }

    MqttClientOptions GetMqttClientOptionsBuilder
    {
        get
        {
            return new MqttClientOptionsBuilder()
               .WithTcpServer(MQConfigRepo.Host, MQConfigRepo.Port)
               .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
               .Build();
        }
    }
}