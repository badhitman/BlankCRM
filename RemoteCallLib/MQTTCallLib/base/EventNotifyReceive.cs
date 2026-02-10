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
        LayoutContainerId = layoutContainerId;
        queueName = $"{LayoutContainerId}_{QueueName.Replace("\\", "/")}";
        _propertiesValues = propertiesValues;
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
        MqttClientSubscribeOptions subscribeOptions = new()
        {
            TopicFilters = [new() { Topic = queueName }],
            UserProperties = [new(Routes.USER_CONTROLLER_NAME, _userInfoBytes ?? Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(null)))]
        };

        if (_propertiesValues is not null && _propertiesValues.Count != 0)
            _propertiesValues.ForEach(x => subscribeOptions.UserProperties.Add(new(x.Key, x.Value)));

        try
        {
            await mqttClient.SubscribeAsync(subscribeOptions, cancellationToken: stoppingToken);
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
        if (mqttClient is null)
            return;

        MqttClientUnsubscribeOptionsBuilder unsubscribeOptions = new MqttClientUnsubscribeOptionsBuilder()
            .WithTopicFilter(queueName);

        List<MqttUserProperty> usrProps = [];
        if (_propertiesValues is not null && _propertiesValues.Count != 0)
            _propertiesValues.ForEach(x => unsubscribeOptions.WithUserProperty(new(x.Key, x.Value)));

        if (!isMute)
            unsubscribeOptions.WithUserProperty(new(Routes.USER_CONTROLLER_NAME, _userInfoBytes ?? Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(null))));
        else
            unsubscribeOptions.WithUserProperty(new(Routes.MUTE_CONTROLLER_NAME, new ReadOnlyMemory<byte>([1])));

        await mqttClient.UnsubscribeAsync(unsubscribeOptions.Build(), cancellationToken: stoppingToken);

        Notify = null;
    }

    MqttClientOptions GetClientOptionsBuilderMQTT(bool isMute)
    {
        MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
               .WithTcpServer(rabbitConf.Host, rabbitConf.Port)
               .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
               ;

        return new MqttClientOptionsBuilder()
               .WithTcpServer(rabbitConf.Host, rabbitConf.Port)
               .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
               .Build();
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await UnregisterAction();
    }
}