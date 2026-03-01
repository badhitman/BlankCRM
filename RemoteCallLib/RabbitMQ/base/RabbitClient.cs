////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using SharedLib;
using MQTTnet;

namespace RemoteCallLib;

/// <summary>
/// Удалённый вызов команд (RabbitMq client)
/// </summary>
public class RabbitClient : IMQStandardClientRPC
{
    readonly RabbitMQConfigModel RabbitConfigRepo;
    readonly ConnectionFactory factory;
    readonly ILogger<RabbitClient> loggerRepo;
    readonly ITraceRabbitActionsServiceTransmission traceRepo;
    readonly MqttClientFactory mqttFactoryCLI = new();
    readonly RealtimeMQTTClientConfigModel MQConfigRepo;
    IConnection? _connection;
    readonly string AppName;

    /// <summary>
    /// Параметры вызывающей очереди
    /// </summary>
    static Dictionary<string, object>? ListenerQueueArguments;

    /// <summary>
    /// Параметры ответной очереди
    /// </summary>
    static Dictionary<string, object?>? ResponseQueueArguments;

    /// <inheritdoc/>
    public static readonly JsonSerializerOptions SerializerOptions = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true };

    /// <summary>
    /// Удалённый вызов команд (RabbitMq client)
    /// </summary>
    public RabbitClient(
        IOptions<RabbitMQConfigModel> rabbitConf,
        ILogger<RabbitClient> _loggerRepo,
        ITraceRabbitActionsServiceTransmission _traceRepo,
        RealtimeMQTTClientConfigModel _MQConfigRepo,
        string appName)
    {
        MQConfigRepo = _MQConfigRepo;
        traceRepo = _traceRepo;
        AppName = appName;
        loggerRepo = _loggerRepo;
        RabbitConfigRepo = rabbitConf.Value;        

        if (ListenerQueueArguments is null)
        {
            ListenerQueueArguments = new() { { "x-queue-type", "quorum" } };

            if (rabbitConf.Value.ListenerMessageTTL.HasValue && rabbitConf.Value.ListenerMessageTTL.Value > 0)
                ListenerQueueArguments.Add("x-message-ttl", rabbitConf.Value.ListenerMessageTTL.Value);

            if (rabbitConf.Value.ListenerConsumerTimeout.HasValue && rabbitConf.Value.ListenerConsumerTimeout.Value > 0)
                ListenerQueueArguments.Add("x-consumer-timeout", rabbitConf.Value.ListenerConsumerTimeout.Value);
        }

        if (ResponseQueueArguments is null)
        {
            ResponseQueueArguments = new() { { "x-queue-type", "classic" } };

            if (rabbitConf.Value.ResponseMessageTTL.HasValue && rabbitConf.Value.ResponseMessageTTL.Value > 0)
                ResponseQueueArguments.Add("x-message-ttl", rabbitConf.Value.ResponseMessageTTL.Value);

            if (rabbitConf.Value.ExpiresResponseQueue.HasValue && rabbitConf.Value.ExpiresResponseQueue.Value > 0)
                ResponseQueueArguments.Add("x-expires", rabbitConf.Value.ExpiresResponseQueue.Value);

            if (rabbitConf.Value.ResponseConsumerTimeout.HasValue && rabbitConf.Value.ResponseConsumerTimeout.Value > 0)
                ResponseQueueArguments.Add("x-consumer-timeout", rabbitConf.Value.ResponseConsumerTimeout.Value);
        }

        factory = new()
        {
            ClientProvidedName = RabbitConfigRepo.ClientProvidedName,
            HostName = RabbitConfigRepo.HostName,
            Port = RabbitConfigRepo.Port,
            UserName = RabbitConfigRepo.UserName,
            Password = RabbitConfigRepo.Password,
        };
    }

    /// <inheritdoc/>
    public async Task<T?> MqRemoteCallAsync<T>(string queue, object? request = null, bool waitResponse = true, CancellationToken tokenOuter = default)
    {
        queue = queue.Replace("\\", "/");
        // Custom ActivitySource for the application
        ActivitySource greeterActivitySource = new($"OTel.{AppName}");
        // Create a new Activity scoped to the method
        using Activity? activity = greeterActivitySource.StartActivity($"OTel.{queue}");

        Meter greeterMeter = new($"OTel.{AppName}", "1.0.0");
        Counter<long> countGreetings = greeterMeter.CreateCounter<long>(GlobalStaticConstantsRoutes.Routes.DURATION_ACTION_NAME, description: "Длительность в мс.");

        activity?.Start();
        string? correlationId = waitResponse ? Guid.NewGuid().ToString() : null;

        try
        {
            if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                {
                    Sender = $"{GetType().Name}.{nameof(MqRemoteCallAsync)} /{nameof(waitResponse)}:{waitResponse}",
                    GuidSession = correlationId ?? "~",
                    ReceiverName = queue,
                    PayloadBody = request,
                    UTCTimestampInitReceive = DateTime.UtcNow,
                }, tokenOuter);
        }
        catch
        {

        }

        string _replyQueueName = waitResponse ? $"{AppName}.{RabbitConfigRepo.QueueMqNamePrefixForResponse}{queue}_{correlationId}".Replace("\\", "/") : "";
        activity?.SetTag(nameof(_replyQueueName), _replyQueueName);

        string msg;
        IMqttClient mqttClient = mqttFactoryCLI.CreateMqttClient();
        IChannel? _channel = null;

        CancellationTokenSource cts = new();
        AsyncEventingBasicConsumer consumer;
        TResponseMQModel<T?>? res_io = null;
        Stopwatch stopwatch = new();
        _connection ??= await factory.CreateConnectionAsync(cancellationToken: CancellationToken.None);
        try
        {
            _channel = await _connection.CreateChannelAsync(cancellationToken: tokenOuter);
            consumer = new(_channel);
        }
        catch (TaskCanceledException ex)
        {
            _channel?.Dispose();

            if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                {
                    Sender = $"{GetType().Name}.{nameof(MqRemoteCallAsync)} /{nameof(waitResponse)}:{waitResponse}",
                    GuidSession = correlationId ?? "~",
                    ReceiverName = nameof(TaskCanceledException),
                    PayloadBody = ex,
                    UTCTimestampInitReceive = DateTime.UtcNow,
                }, tokenOuter);

            return default;
        }
        catch (OperationCanceledException ex)
        {
            _channel?.Dispose();

            if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                {
                    Sender = $"{GetType().Name}.{nameof(MqRemoteCallAsync)} /{nameof(waitResponse)}:{waitResponse}",
                    GuidSession = correlationId ?? "~",
                    ReceiverName = nameof(OperationCanceledException),
                    PayloadBody = ex,
                    UTCTimestampInitReceive = DateTime.UtcNow,
                }, tokenOuter);

            return default;
        }
        catch (Exception ex)
        {
            msg = "exception basic ask. error {295C630E-4069-44A1-8619-15E418F4BF58}";
            loggerRepo.LogError(ex, msg);

            _channel?.Dispose();
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"{GetType().Name}.{nameof(MqRemoteCallAsync)} /{nameof(waitResponse)}:{waitResponse}",
                        GuidSession = correlationId ?? "~",
                        ReceiverName = nameof(Exception),
                        PayloadBody = ex,
                        UTCTimestampInitReceive = DateTime.UtcNow,
                    }, tokenOuter);
            }
            catch
            {

            }
            return default;
        }
        
        async Task MessageReceivedEvent(MqttApplicationMessageReceivedEventArgs e)
        {
            string content = Encoding.UTF8.GetString(e.ApplicationMessage.Payload).Trim();
            try
            {
                res_io = content.Equals("null", StringComparison.OrdinalIgnoreCase)
                ? default
                : JsonConvert.DeserializeObject<TResponseMQModel<T?>?>(content);
                e.ReasonCode = MqttApplicationMessageReceivedReasonCode.Success;
                e.ResponseReasonString = typeof(T).Name;
            }
            catch (Exception ex)
            {
                loggerRepo.LogError(ex, $"Ошибка выполнения удалённой команды: {_replyQueueName}");
                e.ReasonCode = MqttApplicationMessageReceivedReasonCode.UnspecifiedError;
                e.ResponseReasonString = ex.Message;
            }

            if (!content.Contains(GlobalStaticConstantsRoutes.Routes.PASSWORD_CONTROLLER_NAME, StringComparison.OrdinalIgnoreCase))
                activity?.SetBaggage(nameof(content), content);
            else
                activity?.SetBaggage(nameof(content), $"[hide data]: `{Routes.PASSWORD_CONTROLLER_NAME}` - contains");

            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"{GetType().Name}.{nameof(MqRemoteCallAsync)}.{nameof(MessageReceivedEvent)}",
                        GuidSession = correlationId ?? "~",
                        ReceiverName = queue,
                        PayloadBody = content,
                        UTCTimestampInitReceive = DateTime.UtcNow,
                    }, tokenOuter);
            }
            catch
            {

            }
            countGreetings.Add(res_io.Duration().Milliseconds);

            stopwatch.Stop();
            cts.Cancel();
            cts.Dispose();
            await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
        }

        CancellationToken token = cts.Token;
        ManualResetEventSlim mres = new(false);
        BasicProperties properties = new() { AppId = AppName, CorrelationId = correlationId };
        if (waitResponse)
        {
            mqttClient.ApplicationMessageReceivedAsync += MessageReceivedEvent;

            await mqttClient.ConnectAsync(GetMqttClientOptionsBuilder(new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1])), token);
            await mqttClient.SubscribeAsync(_replyQueueName, cancellationToken: token);

            properties.ReplyTo = string.IsNullOrEmpty(_replyQueueName) ? null : _replyQueueName;
        }

        try
        {
            await _channel!.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: ListenerQueueArguments!, cancellationToken: tokenOuter);
        }
        catch (TaskCanceledException ex)
        {
            _channel.Dispose();
            await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(TaskCanceledException)}]",
                        GuidSession = correlationId ?? "~",
                        ReceiverName = queue,
                        PayloadBody = ex,
                        UTCTimestampInitReceive = DateTime.UtcNow,
                    }, tokenOuter);
            }
            catch
            {

            }
            return default;
        }
        catch (OperationCanceledException ex)
        {
            _channel.Dispose();
            await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(OperationCanceledException)}]",
                        GuidSession = correlationId ?? "~",
                        ReceiverName = queue,
                        PayloadBody = ex,
                        UTCTimestampInitReceive = DateTime.UtcNow,
                    }, tokenOuter);
            }
            catch
            {

            }

            return default;
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, "exception 52301C76-8A66-466B-9553-56D44711135A");
            await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
            _channel.Dispose();
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(Exception)}]",
                        GuidSession = correlationId ?? "~",
                        ReceiverName = queue,
                        PayloadBody = ex,
                        UTCTimestampInitReceive = DateTime.UtcNow,
                    }, tokenOuter);
            }
            catch
            {

            }
            return default;
        }

        string request_payload_json = "";
#if DEBUG
        try
        {
            request_payload_json = JsonConvert.SerializeObject(request, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка сериализации объекта [{request?.GetType().Name}]: {request}");
            _channel.Dispose();
            await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
            return default;
        }

        byte[] body = request is null ? [] : Encoding.UTF8.GetBytes(request_payload_json);
#else
        byte[] body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(request, SerializerOptions);
#endif

        try
        {
            await _channel!.BasicPublishAsync(exchange: "",
                            routingKey: queue,
                            mandatory: true,
                            basicProperties: properties,
                            body: body,
                            cancellationToken: tokenOuter);
        }
        catch (TaskCanceledException ex)
        {
            _channel.Dispose();
            await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(TaskCanceledException)}]",
                        GuidSession = correlationId ?? "~",
                        ReceiverName = queue,
                        PayloadBody = ex,
                        UTCTimestampInitReceive = DateTime.UtcNow,
                    }, tokenOuter);
            }
            catch
            {

            }
            return default;
        }
        catch (OperationCanceledException ex)
        {
            _channel.Dispose();
            await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(OperationCanceledException)}]",
                        GuidSession = correlationId ?? "~",
                        ReceiverName = queue,
                        PayloadBody = ex,
                        UTCTimestampInitReceive = DateTime.UtcNow,
                    }, tokenOuter);
            }
            catch
            {

            }
            return default;
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, "exception 4BDEC834-2CA1-42D6-9A69-9F3700F064C2");
            _channel.Dispose();
            await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(Exception)}]",
                        GuidSession = correlationId ?? "~",
                        ReceiverName = queue,
                        PayloadBody = ex,
                        UTCTimestampInitReceive = DateTime.UtcNow,
                    }, tokenOuter);
            }
            catch
            {

            }
            return default;
        }

        if (waitResponse)
        {
            //tokenOuter.Register(cts.Cancel);
            stopwatch.Start();
            _ = Task.Run(async () =>
            {
                await Task.Delay(RabbitConfigRepo.RemoteCallTimeoutMs, token);
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = nameof(RabbitConfigRepo.RemoteCallTimeoutMs),
                            GuidSession = correlationId ?? "~",
                            ReceiverName = queue,
                            PayloadBody = res_io,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }

                cts.Cancel();
                await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
                //cts.Dispose();
            }, token);
            try
            {
                mres.Wait(token);
            }
            catch (TaskCanceledException ex)
            {
                loggerRepo.LogDebug($"response for {_replyQueueName}");
                _channel.Dispose();
                await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = $"[{GetType().Name}]-[{nameof(TaskCanceledException)}]",
                            GuidSession = correlationId ?? "~",
                            ReceiverName = queue,
                            PayloadBody = ex,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }
            }
            catch (OperationCanceledException ex)
            {
                loggerRepo.LogDebug($"response for {_replyQueueName}");
                _channel.Dispose();
                await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = $"[{GetType().Name}]-[{nameof(OperationCanceledException)}]",
                            GuidSession = correlationId ?? "~",
                            ReceiverName = queue,
                            PayloadBody = ex,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }
            }
            catch (Exception ex)
            {
                msg = "exception Wait response. error {8B621451-2214-467F-B8E9-906DD866662C}";
                loggerRepo.LogError(ex, msg);
                stopwatch.Stop();
                _channel.Dispose();
                await mqttClient.DisconnectAsync(cancellationToken: CancellationToken.None);
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = $"[{GetType().Name}]-[{nameof(Exception)}]",
                            GuidSession = correlationId ?? "~",
                            ReceiverName = queue,
                            PayloadBody = ex,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }

                return default;
            }

            if (stopwatch.IsRunning)
            {
                msg = $"Elapsed for `{queue}` -> `{_replyQueueName}`: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(RabbitConfigRepo.RemoteCallTimeoutMs)}\n{request_payload_json}";
                loggerRepo.LogError(msg);
                stopwatch.Stop();
            }
            else
                loggerRepo.LogDebug($"Elapsed [{queue}] -> [{_replyQueueName}]: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(RabbitConfigRepo.RemoteCallTimeoutMs)}");
        }
        else
            return default;

        activity?.Stop();

        if (typeof(T) != typeof(object) && (res_io is null || res_io.Response is null))
        {
            msg = $"Response MQ/IO is null [{queue}] -> [{_replyQueueName}]: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(RabbitConfigRepo.RemoteCallTimeoutMs)}";
            loggerRepo.LogError(msg);

            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = typeof(T).Name,
                        GuidSession = correlationId ?? "~",
                        ReceiverName = queue,
                        PayloadBody = res_io,
                        UTCTimestampInitReceive = DateTime.UtcNow,
                    }, tokenOuter);
            }
            catch
            {

            }

            return default;
        }
        else if (res_io is null)
            return default;

        try
        {
            if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                {
                    Sender = "Success",
                    GuidSession = correlationId ?? "~",
                    ReceiverName = queue,
                    PayloadBody = res_io,
                    UTCTimestampInitReceive = DateTime.UtcNow,
                }, tokenOuter);
        }
        catch
        {

        }

        return res_io.Response;

        MqttClientOptions GetMqttClientOptionsBuilder(KeyValuePair<string, byte[]>? propertyValue)
        {
            MqttClientOptionsBuilder res = new MqttClientOptionsBuilder()
                   .WithTcpServer(MQConfigRepo.Host, MQConfigRepo.Port)
                   .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                   .WithClientId($"{queue} [{typeof(T).Name}] {Guid.NewGuid()}")
                   ;

            if (propertyValue is not null)
                res.WithUserProperty(propertyValue.Value.Key, propertyValue.Value.Value);

            return res.Build();
        }
    }

}