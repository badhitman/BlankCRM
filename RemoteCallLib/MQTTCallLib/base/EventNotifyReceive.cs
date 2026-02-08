////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Packets;
using Newtonsoft.Json;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    string? LayoutContainerId;
    List<KeyValuePair<string, byte[]>>? _propertiesValues;

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
    public async Task RegisterAction(string QueueName, Action<T> actNotify, string layoutContainerId, byte[]? userInfoBytes, bool isMute = false, List<KeyValuePair<string, byte[]>>? propertiesValues = null, CancellationToken stoppingToken = default)
    {
        QueueName = QueueName.Replace("\\", "/");
        _propertiesValues = propertiesValues;
        _userInfoBytes = userInfoBytes;
        LayoutContainerId = layoutContainerId;
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
        List<MqttUserProperty> usrProps = [];
        if (_propertiesValues is not null)
            usrProps.AddRange(_propertiesValues.Select(x => new MqttUserProperty(x.Key, x.Value)));

        if (mqttClient.IsConnected)
        {
            if (!isMute)
            {
                usrProps.Add(new(Routes.USER_CONTROLLER_NAME, _userInfoBytes ?? Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(null))));
                await mqttClient.DisconnectAsync(new() { UserProperties = usrProps }, stoppingToken);
            }
            else
            {
                usrProps.Add(new(Routes.MUTE_CONTROLLER_NAME, new ReadOnlyMemory<byte>([1])));
                await mqttClient.DisconnectAsync(new() { UserProperties = usrProps }, stoppingToken);
            }
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

        if (_propertiesValues is not null)
            foreach (var userProp in _propertiesValues)
                builder.WithUserProperty(userProp.Key, new ReadOnlyMemory<byte>(userProp.Value));

        return builder.Build();
    }
}