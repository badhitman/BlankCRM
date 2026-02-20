////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using MQTTnet.Diagnostics.Logger;
using System.Threading.Tasks;
using MQTTnet.Exceptions;
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

    string?
        LayoutContainerId, queueName;

    byte[]? _userInfoBytes;
    readonly RealtimeMQTTClientConfigModel MQConfigRepo;
    readonly ILogger<EventNotifyReceive<T>> LoggerRepo;

    List<KeyValuePair<string, byte[]>>? _propertiesValues;

    /// <summary>
    /// EventNotifyReceive
    /// </summary>
    public EventNotifyReceive(
        RealtimeMQTTClientConfigModel rabbitConf,
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
        mqttClient ??= mqttFactory.CreateMqttClient(new ConsoleLogger(LoggerRepo));
        mqttClient.ApplicationMessageReceivedAsync += ApplicationMessageReceived;

        try
        {
            await mqttClient.ConnectAsync(GetMqttClientOptionsBuilder(queueName, isMute), stoppingToken);

            MqttClientSubscribeOptions subOpt = new()
            {
                TopicFilters = [new() { Topic = queueName, QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce }],
                UserProperties = [new(Routes.OBJECT_CONTROLLER_NAME, Encoding.UTF8.GetBytes(LayoutContainerId))]
            };

            await mqttClient.SubscribeAsync(subOpt, cancellationToken: stoppingToken);
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
            MqttClientUnsubscribeOptions unsubscribeOptions = new()
            {
                TopicFilters = [queueName],
                UserProperties = [new(Routes.OBJECT_CONTROLLER_NAME, Encoding.UTF8.GetBytes(LayoutContainerId!))]
            };

            if (mqttClient.IsConnected)
            {
                try
                {
                    await mqttClient.UnsubscribeAsync(unsubscribeOptions, stoppingToken);
                }
                catch (MqttClientDisconnectedException)
                {
                    mqttClient?.Dispose();
                    return;
                }
                catch (TaskCanceledException)
                {
                    mqttClient?.Dispose();
                    return;
                }
                catch (OperationCanceledException)
                {
                    mqttClient?.Dispose();
                    return;
                }
            }
            else
            {
                mqttClient?.Dispose();
                return;
            }

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
               .WithClientId($"{queueName} [{nameof(EventNotifyReceive<>)}.{typeof(T).Name}] {LayoutContainerId}")
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

    sealed class ConsoleLogger(ILogger<EventNotifyReceive<T>> LoggerRepo) : IMqttNetLogger
    {
        public bool IsEnabled => true;

        public void Publish(MqttNetLogLevel logLevel, string source, string message, object[]? parameters, Exception? exception)
        {
            switch (logLevel)
            {
                case MqttNetLogLevel.Verbose:
                    //LoggerRepo.LogDebug(message);
                    break;

                case MqttNetLogLevel.Info:
                    //LoggerRepo.LogInformation(message);
                    break;

                case MqttNetLogLevel.Warning:
                    LoggerRepo.LogWarning(message);
                    break;

                case MqttNetLogLevel.Error:
                    LoggerRepo.LogError(message);
                    break;
            }
        }
    }
}