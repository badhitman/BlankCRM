////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
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
public class EventNotifyReceive<T>(
    MQTTClientConfigModel rabbitConf,
    ILogger<EventNotifyReceive<T>> loggerRepo) : IEventNotifyReceive<T>, IAsyncDisposable
{
    /// <summary>
    /// Notify
    /// </summary>
    public event IEventNotifyReceive<T>.AccountHandler? Notify;

    static IMqttClient? mqttClient;
    MqttClientFactory mqttFactory = new();
    byte[]? _userInfoBytes;
    string?
        LayoutContainerId,
        queueName;

    List<KeyValuePair<string, byte[]>>? _propertiesValues;

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
                loggerRepo.LogError(ex, $"Ошибка выполнения удалённой команды: {queueName}");
                //
                e.ReasonCode = MqttApplicationMessageReceivedReasonCode.UnspecifiedError;
                e.ResponseReasonString = ex.Message;
            }
            if (sr is null)
                loggerRepo.LogError($"Ошибка обработки удалённой команды (source is null): {queueName}");
            else
            {
                actNotify(sr);
                if (Notify is not null)
                    Notify(sr);
            }
            return Task.CompletedTask;
        }

        if (mqttClient is null)
        {
            mqttClient = mqttFactory.CreateMqttClient();
            await mqttClient.ConnectAsync(GetClientOptionsBuilderMQTT(isMute), stoppingToken);
        }

        mqttClient.ApplicationMessageReceivedAsync += ApplicationMessageReceived;

        try
        {
            await mqttClient.SubscribeAsync($"{queueName}_{LayoutContainerId}", cancellationToken: stoppingToken);
            loggerRepo.LogTrace($"{nameof(queueName)}:{queueName}");
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"can`t connect/subscribe: `{queueName}`");
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
        await mqttClient.UnsubscribeAsync($"{queueName}_{LayoutContainerId}", cancellationToken: stoppingToken);
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

    MqttClientOptions GetClientOptionsBuilderMQTT(bool isMute)
    {
        MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
               .WithTcpServer(rabbitConf.Host, rabbitConf.Port)
               .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
               .WithUserProperty(Routes.USER_CONTROLLER_NAME, _userInfoBytes ?? Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(null)));

        if (isMute)
            builder.WithUserProperty(Routes.MUTE_CONTROLLER_NAME, new ReadOnlyMemory<byte>([1]));

        if (_propertiesValues is not null)
            foreach (KeyValuePair<string, byte[]> userProp in _propertiesValues)
                builder.WithUserProperty(userProp.Key, new ReadOnlyMemory<byte>(userProp.Value));

        return builder.Build();
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await UnregisterAction();
    }
}