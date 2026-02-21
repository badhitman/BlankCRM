////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using MQTTnet;
using System;

namespace SharedLib;

/// <summary>
/// Удалённый вызов команд (RabbitMq client)
/// </summary>
/// <inheritdoc/>
public class ClientMQTT(RealtimeMQTTClientConfigModel mqConf, ILogger<ClientMQTT> _loggerRepo, string appName) : IMQStandardClientExtRPC
{
    readonly RealtimeMQTTClientConfigModel MQConfigRepo = mqConf;
    MqttClientFactory mqttFactory = new();

    readonly ILogger<ClientMQTT> loggerRepo = _loggerRepo;

    readonly string AppName = appName;

    /// <inheritdoc/>
    public static readonly JsonSerializerOptions SerializerOptions = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true };

    /// <inheritdoc/>
    public async Task<T?> MqRemoteCallAsync<T>(string queue, object? request = null, bool waitResponse = true, KeyValuePair<string, byte[]>? propertyValue = null, CancellationToken tokenOuter = default) where T : class
    {
        queue = queue.Replace("\\", "/");
        using IMqttClient mqttClient = mqttFactory.CreateMqttClient();
        //using IMqttClient? responseClient = waitResponse ? mqttFactory.CreateMqttClient() : null;

        string _sc = MQConfigRepo.ToString();

        Meter greeterMeter = new($"OTel.{AppName}", "1.0.0");
        Counter<long> countGreetings = greeterMeter.CreateCounter<long>(GlobalStaticConstantsRoutes.Routes.DURATION_ACTION_NAME, description: "Длительность в мс.");

        Stopwatch stopwatch = new();
        CancellationTokenSource cts = new();
        CancellationToken token = cts.Token;
        ManualResetEventSlim mres = new(false);
        string _msg;
        TResponseMQModel<T?>? res_io = null;
        string request_payload_json = "";
        try
        {
            request_payload_json = JsonConvert.SerializeObject(request, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка сериализации объекта [{request?.GetType().Name}]: {request}");
        }

        string response_topic = waitResponse ? $"{MQConfigRepo.QueueMqNamePrefixForResponse}{queue}_{Guid.NewGuid()}" : "";

        Task ResponseClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eMsg)
        {
            mqttClient.ApplicationMessageReceivedAsync -= ResponseClient_ApplicationMessageReceivedAsync;
            string msg;
            string content = Encoding.UTF8.GetString(eMsg.ApplicationMessage.Payload);

            try
            {
                res_io = JsonConvert.DeserializeObject<TResponseMQModel<T?>>(content, GlobalStaticConstants.JsonSerializerSettings)
                    ?? throw new Exception("parse error {0CBCCD44-63C8-4E93-8349-11A8BE63B235}");

                if (!res_io.Success())
                    loggerRepo.LogError(res_io.Message());

                countGreetings.Add(res_io.Duration().Milliseconds);
            }
            catch (Exception ex)
            {
                msg = $"error deserialisation: {content}.\n\nerror ";
                loggerRepo.LogError(ex, msg);
            }

            stopwatch.Stop();
            cts.Cancel();
            cts.Dispose();

            return Task.CompletedTask;
        }
        mqttClient?.ApplicationMessageReceivedAsync += ResponseClient_ApplicationMessageReceivedAsync;
        loggerRepo.LogTrace($"Sending message into queue [{queue}]", request_payload_json);

        MqttClientConnectResult res = await mqttClient!.ConnectAsync(GetMqttClientOptionsBuilder(queue, propertyValue), tokenOuter);

        MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(queue)
            .WithPayload(request_payload_json)
            .WithResponseTopic(response_topic)
            .Build();

        MqttClientDisconnectOptions dq = new();
        if (propertyValue is not null)
        {
            dq.UserProperties ??= [];
            dq.UserProperties.Add(new(propertyValue.Value.Key, propertyValue.Value.Value));
        }

        if (waitResponse)
        {
            stopwatch.Start();
            try
            {
                await mqttClient.ConnectAsync(GetMqttClientOptionsBuilder(queue, propertyValue), tokenOuter);
                await mqttClient.SubscribeAsync(response_topic, cancellationToken: tokenOuter);
                await mqttClient.PublishAsync(applicationMessage, tokenOuter);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _msg = $"Request MQ/IO error [{queue}]";
                loggerRepo.LogError(ex, _msg);

                await mqttClient.DisconnectAsync(dq, tokenOuter);
                return default;
            }

            List<Task> tasks = [
                Task.Run(async () => { await Task.Delay(MQConfigRepo.RemoteCallTimeoutMs); cts.Cancel(); }, tokenOuter),
                Task.Run(() => {
                    try
                    {
                        mres.Wait(token);
                    }
                    catch (OperationCanceledException)
                    {
                        loggerRepo.LogDebug($"response for {response_topic}");
                    }
                    catch (Exception ex)
                    {
                        _msg = "exception Wait response. error {8B621451-2214-467F-B8E9-906DD866662C}";
                        loggerRepo.LogError(ex, _msg);
                        stopwatch.Stop();
                    }
                }, tokenOuter)
                ];

            await Task.WhenAny(tasks);

            if (stopwatch.IsRunning)
            {
                _msg = $"Elapsed for `{queue}`: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(MQConfigRepo.RemoteCallTimeoutMs)}";
                loggerRepo.LogError(_msg);
                stopwatch.Stop();
            }
            else
                loggerRepo.LogDebug($"Elapsed [{queue}]: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(MQConfigRepo.RemoteCallTimeoutMs)}");
        }
        else
        {
            await mqttClient.PublishAsync(applicationMessage, tokenOuter);
            if (mqttClient?.IsConnected == true)
                await mqttClient.DisconnectAsync(dq, tokenOuter);
            return default;
        }

        if (typeof(T) != typeof(object) && (res_io is null || res_io.Response is null))
        {
            _msg = $"Response MQ/IO is null [{queue}]: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(MQConfigRepo.RemoteCallTimeoutMs)}";
            loggerRepo.LogError(_msg);
            await mqttClient.DisconnectAsync(dq, tokenOuter);
            return default;
        }
        else if (res_io is null)
        {
            await mqttClient.DisconnectAsync(dq, tokenOuter);
            return default;
        }

        await mqttClient.DisconnectAsync(dq, tokenOuter);
        return res_io.Response;
    }

    MqttClientOptions GetMqttClientOptionsBuilder(string queue, KeyValuePair<string, byte[]>? propertyValue)
    {
        MqttClientOptionsBuilder res = new MqttClientOptionsBuilder()
               .WithTcpServer(MQConfigRepo.Host, MQConfigRepo.Port)
               .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
               .WithClientId($"{queue} [{GetType().Name}] {Guid.NewGuid()}");

        if (propertyValue is not null)
            res.WithUserProperty(propertyValue.Value.Key, propertyValue.Value.Value);

        return res.Build();
    }
}