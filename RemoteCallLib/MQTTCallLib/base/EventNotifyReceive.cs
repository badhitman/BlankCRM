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
using System.Security.Cryptography;
using static SharedLib.GlobalStaticConstantsRoutes;

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

    readonly MQTTClientConfigModel MQConfigRepo;
    readonly ILogger<EventNotifyReceive<T>> LoggerRepo;
    byte[]? _userInfoBytes;

    /// <summary>
    /// EventNotifyReceive
    /// </summary>
    public EventNotifyReceive(
        MQTTClientConfigModel rabbitConf,
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
    public async Task RegisterAction(string QueueName, Action<T> actNotify, byte[]? userInfoBytes, bool isMute = false, CancellationToken stoppingToken = default)
    {
        _userInfoBytes = userInfoBytes;
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
                actNotify(sr);
                if (Notify is not null)
                    Notify(sr);
            }
            return Task.CompletedTask;
        }

        mqttClient.ApplicationMessageReceivedAsync += ApplicationMessageReceived;

        try
        {
            await mqttClient.ConnectAsync(GetMqttClientOptionsBuilder(isMute), stoppingToken);
            await mqttClient.SubscribeAsync(QueueName, cancellationToken: stoppingToken);
            LoggerRepo.LogTrace($"QueueName:{QueueName}");
        }
        catch (Exception ex)
        {
            LoggerRepo.LogError(ex, "can`t connect/subscribe");
            mqttClient.ApplicationMessageReceivedAsync -= ApplicationMessageReceived;
            return;
        }
    }

    /// <inheritdoc/>
    public async Task UnregisterAction(bool isMute = false, CancellationToken stoppingToken = default)
    {
        if (mqttClient.IsConnected)
        {
            if (!isMute)
                await mqttClient.DisconnectAsync(new() { UserProperties = [new(Routes.USER_CONTROLLER_NAME, _userInfoBytes ?? Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(null)))] }, stoppingToken);
            else
                await mqttClient.DisconnectAsync(new() { UserProperties = [new(Routes.MUTE_CONTROLLER_NAME, new ReadOnlyMemory<byte>([1]))] }, stoppingToken);
        }
        mqttClient.Dispose();
        Notify = null;
    }

    MqttClientOptions GetMqttClientOptionsBuilder(bool isMute)
    {
        MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
               .WithTcpServer(MQConfigRepo.Host, MQConfigRepo.Port)
               .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
               .WithUserProperty(Routes.USER_CONTROLLER_NAME, _userInfoBytes ?? Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(null)));

        if (isMute)
            builder.WithUserProperty(Routes.MUTE_CONTROLLER_NAME, new ReadOnlyMemory<byte>([1]));

        return builder.Build();
    }
}