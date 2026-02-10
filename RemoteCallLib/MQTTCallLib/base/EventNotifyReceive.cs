////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MQTTnet.Packets;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using SharedLib;
using MQTTnet;
using System;

namespace RemoteCallLib;

/// <summary>
/// EventNotifyReceive
/// </summary>
public class EventNotifyReceive<T> : IEventNotifyReceive<T>, IAsyncDisposable
{
    /// <summary>
    /// Notify
    /// </summary>
    public event IEventNotifyReceive<T>.AccountHandler? Notify;

    IMqttClient? mqttClient;
    MqttClientFactory mqttFactory = new();

    readonly MQTTClientConfigModel MQConfigRepo;
    readonly ILogger<EventNotifyReceive<T>> LoggerRepo;
    byte[]? _userInfoBytes;
    string? LayoutContainerId;
    List<KeyValuePair<string, byte[]>>? _propertiesValues;
    string? queueName;

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
    }

    /// <inheritdoc/>
    public async Task RegisterAction(string QueueName, Action<T> actNotify, string layoutContainerId, byte[]? userInfoBytes, bool isMute = false, List<KeyValuePair<string, byte[]>>? propertiesValues = null, CancellationToken stoppingToken = default)
    {
        queueName = QueueName.Replace("\\", "/");
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
                e.ReasonCode = MqttApplicationMessageReceivedReasonCode.Success;
                e.ResponseReasonString = typeof(T).Name;
            }
            catch (Exception ex)
            {
                LoggerRepo.LogError(ex, $"Ошибка выполнения удалённой команды: {queueName}");
                //
                e.ReasonCode = MqttApplicationMessageReceivedReasonCode.UnspecifiedError;
                e.ResponseReasonString = ex.Message;
            }
            if (sr is null)
                LoggerRepo.LogError($"Ошибка обработки удалённой команды (source is null): {queueName}");
            else
            {
                actNotify(sr);
                if (Notify is not null)
                    Notify(sr);
            }
            return Task.CompletedTask;
        }
        mqttClient = mqttFactory.CreateMqttClient();
        mqttClient.ApplicationMessageReceivedAsync += ApplicationMessageReceived;

        try
        {
            await mqttClient.ConnectAsync(GetMqttClientOptionsBuilder(queueName, isMute), stoppingToken);
            await mqttClient.SubscribeAsync(queueName, cancellationToken: stoppingToken);
            LoggerRepo.LogTrace($"{nameof(queueName)}:{queueName}");
        }
        catch (Exception ex)
        {
            LoggerRepo.LogError(ex, $"can`t connect/subscribe: `{queueName}`");
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

        if (mqttClient?.IsConnected == true)
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
        mqttClient?.Dispose();
        Notify = null;
    }

    MqttClientOptions GetMqttClientOptionsBuilder(string queueName, bool isMute)
    {
        MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
               .WithTcpServer(MQConfigRepo.Host, MQConfigRepo.Port)
               .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
               .WithClientId($"{queueName} [{nameof(EventNotifyReceive<>)}.{typeof(T).Name}] {Guid.NewGuid()}")
               .WithUserProperty(Routes.USER_CONTROLLER_NAME, _userInfoBytes ?? Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(null)));

        if (isMute)
            builder.WithUserProperty(Routes.MUTE_CONTROLLER_NAME, new ReadOnlyMemory<byte>([1]));

        if (_propertiesValues is not null)
            foreach (var userProp in _propertiesValues)
                builder.WithUserProperty(userProp.Key, new ReadOnlyMemory<byte>(userProp.Value));

        return builder.Build();
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await UnregisterAction();
    }
}