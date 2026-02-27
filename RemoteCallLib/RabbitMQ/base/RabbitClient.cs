////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Exceptions;
using System.Diagnostics.Metrics;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using SharedLib;

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

    readonly string AppName;

    /// <summary>
    /// Параметры вызывающей очереди
    /// </summary>
    static Dictionary<string, object>? ListenerQueueArguments;

    /// <summary>
    /// Параметры ответной очереди
    /// </summary>
    static Dictionary<string, object>? ResponseQueueArguments;

    /// <inheritdoc/>
    public static readonly JsonSerializerOptions SerializerOptions = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true };

    /// <summary>
    /// Удалённый вызов команд (RabbitMq client)
    /// </summary>
    public RabbitClient(
        IOptions<RabbitMQConfigModel> rabbitConf,
        ILogger<RabbitClient> _loggerRepo,
        ITraceRabbitActionsServiceTransmission _traceRepo,
        string appName)
    {
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
            ResponseQueueArguments = new() { { "x-queue-type", "quorum" } };

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
        string guidRequest = waitResponse ? Guid.NewGuid().ToString() : "~";

        try
        {
            if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                {
                    Sender = $"{GetType().Name}.{nameof(MqRemoteCallAsync)} /{nameof(waitResponse)}:{waitResponse}",
                    GuidSession = guidRequest,
                    ReceiverName = queue,
                    PayloadBody = request,
                    UTCTimestampInitReceive = DateTime.UtcNow,
                }, tokenOuter);
        }
        catch
        {

        }

        string response_topic = waitResponse ? $"{AppName}.{RabbitConfigRepo.QueueMqNamePrefixForResponse}{queue}_{guidRequest}".Replace("\\", "/") : "";
        activity?.SetTag(nameof(response_topic), response_topic);

        string msg;
        IConnection? _connection = null;
        IChannel? _channel = null;

        CancellationTokenSource cts = new();
        AsyncEventingBasicConsumer consumer;
        TResponseMQModel<T?>? res_io = null;
        Stopwatch stopwatch = new();

        try
        {
            _connection = await factory.CreateConnectionAsync(tokenOuter);
            _channel = await _connection.CreateChannelAsync(cancellationToken: tokenOuter);
            consumer = new(_channel);
        }
        catch (TaskCanceledException ex)
        {
            _channel?.Dispose();
            _connection?.Dispose();

            if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                {
                    Sender = $"{GetType().Name}.{nameof(MqRemoteCallAsync)} /{nameof(waitResponse)}:{waitResponse}",
                    GuidSession = guidRequest,
                    ReceiverName = nameof(TaskCanceledException),
                    PayloadBody = ex,
                    UTCTimestampInitReceive = DateTime.UtcNow,
                }, tokenOuter);

            return default;
        }
        catch (OperationCanceledException ex)
        {
            _channel?.Dispose();
            _connection?.Dispose();

            if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                {
                    Sender = $"{GetType().Name}.{nameof(MqRemoteCallAsync)} /{nameof(waitResponse)}:{waitResponse}",
                    GuidSession = guidRequest,
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
            _connection?.Dispose();
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"{GetType().Name}.{nameof(MqRemoteCallAsync)} /{nameof(waitResponse)}:{waitResponse}",
                        GuidSession = guidRequest,
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

        async Task MessageReceivedEvent(object? sender, BasicDeliverEventArgs e)
        {
            string msg;
            consumer.ReceivedAsync -= MessageReceivedEvent;
            string content = Encoding.UTF8.GetString(e.Body.ToArray());

            await _channel.QueuePurgeAsync(response_topic, CancellationToken.None);
            await _channel.QueueDeleteAsync(response_topic, false, false, true, cancellationToken: CancellationToken.None);

            if (!content.Contains(GlobalStaticConstantsRoutes.Routes.PASSWORD_CONTROLLER_NAME, StringComparison.OrdinalIgnoreCase))
                activity?.SetBaggage(nameof(content), content);
            else
                activity?.SetBaggage(nameof(content), $"[hide data]: `{GlobalStaticConstantsRoutes.Routes.PASSWORD_CONTROLLER_NAME}` - contains");

            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"{GetType().Name}.{nameof(MqRemoteCallAsync)}.{nameof(MessageReceivedEvent)}",
                        GuidSession = guidRequest,
                        ReceiverName = queue,
                        PayloadBody = content,
                        UTCTimestampInitReceive = DateTime.UtcNow,
                    }, tokenOuter);
            }
            catch
            {

            }

            try
            {
                res_io = JsonConvert.DeserializeObject<TResponseMQModel<T?>>(content, GlobalStaticConstants.JsonSerializerSettings)
                    ?? throw new Exception("parse error {0CBCCD44-63C8-4E93-8349-11A8BE63B235}");

                if (!res_io.Success())
                    loggerRepo.LogError(res_io.Message());

                countGreetings.Add(res_io.Duration().Milliseconds);
            }
            catch (TaskCanceledException ex)
            {
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = $"[{nameof(MessageReceivedEvent)}]-[{nameof(TaskCanceledException)}]",
                            GuidSession = guidRequest,
                            ReceiverName = nameof(TaskCanceledException),
                            PayloadBody = ex,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }
                return;
            }
            catch (OperationCanceledException ex)
            {
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = $"[{nameof(MessageReceivedEvent)}]-[{nameof(OperationCanceledException)}]",
                            GuidSession = guidRequest,
                            ReceiverName = nameof(OperationCanceledException),
                            PayloadBody = ex,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }
                return;
            }
            catch (Exception ex)
            {
                msg = $"error deserialisation: {content}.\n\nerror ";
                loggerRepo.LogError(ex, msg);
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = nameof(MessageReceivedEvent),
                            GuidSession = guidRequest,
                            ReceiverName = nameof(Exception),
                            PayloadBody = res_io,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }
                return;
            }

            try
            {
                await _channel.BasicAckAsync(e.DeliveryTag, false, tokenOuter);
            }
            catch (TaskCanceledException ex)
            {
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = nameof(MessageReceivedEvent),
                            GuidSession = guidRequest,
                            ReceiverName = nameof(TaskCanceledException),
                            PayloadBody = ex,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }
                return;
            }
            catch (OperationCanceledException ex)
            {
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = nameof(MessageReceivedEvent),
                            GuidSession = guidRequest,
                            ReceiverName = nameof(OperationCanceledException),
                            PayloadBody = ex,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }
                return;
            }
            catch (Exception ex)
            {
                msg = "exception basic ask. error {A62029D4-1A23-461D-99AD-349C6B7500A8}";
                loggerRepo.LogError(ex, msg);
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = nameof(MessageReceivedEvent),
                            GuidSession = guidRequest,
                            ReceiverName = nameof(Exception),
                            PayloadBody = ex,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }
                return;
            }

            stopwatch.Stop();
            cts.Cancel();
            cts.Dispose();
        }

        CancellationToken token = cts.Token;
        ManualResetEventSlim mres = new(false);
        BasicProperties? properties = new() { AppId = AppName };
        if (waitResponse)
        {
            consumer.ReceivedAsync += MessageReceivedEvent;

            properties.ReplyTo = string.IsNullOrEmpty(response_topic) ? null : response_topic;
            try
            {
                await _channel.QueueDeclareAsync(queue: response_topic, durable: false, exclusive: false, autoDelete: false, arguments: ResponseQueueArguments!, cancellationToken: tokenOuter);
                await _channel.BasicConsumeAsync(response_topic, false, consumer, cancellationToken: tokenOuter);
            }
            catch (TaskCanceledException ex)
            {
                _channel.Dispose();
                _connection.Dispose();

                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = nameof(MqRemoteCallAsync),
                            GuidSession = guidRequest,
                            ReceiverName = nameof(TaskCanceledException),
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
                _channel?.Dispose();
                _connection?.Dispose();

                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = nameof(MqRemoteCallAsync),
                            GuidSession = guidRequest,
                            ReceiverName = nameof(OperationCanceledException),
                            PayloadBody = ex,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }

                return default;
            }
            catch (OperationInterruptedException ex)
            {
                msg = $"exception basic ask for [queue: {response_topic}]. error 56AA49DF-E8F8-489F-A2AB-591511EE7B33";
                loggerRepo.LogError(ex, msg);

                _channel?.Dispose();
                _connection?.Dispose();

                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = nameof(MqRemoteCallAsync),
                            GuidSession = guidRequest,
                            ReceiverName = nameof(OperationInterruptedException),
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
                msg = "exception basic ask. error {C8C5AB97-CE68-4A5B-BB7D-FA71C6419A3E}";
                loggerRepo.LogError(ex, msg);

                _channel.Dispose();
                _connection.Dispose();
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = nameof(MqRemoteCallAsync),
                            GuidSession = guidRequest,
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
        }

        try
        {
            await _channel!.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: ListenerQueueArguments!, cancellationToken: tokenOuter);
        }
        catch (TaskCanceledException ex)
        {
            _channel.Dispose();
            _connection.Dispose();
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(TaskCanceledException)}]",
                        GuidSession = guidRequest,
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
            _connection.Dispose();

            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(OperationCanceledException)}]",
                        GuidSession = guidRequest,
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

            _channel.Dispose();
            _connection.Dispose();
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(Exception)}]",
                        GuidSession = guidRequest,
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
            _connection.Dispose();

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
            _connection.Dispose();
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(TaskCanceledException)}]",
                        GuidSession = guidRequest,
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
            _connection.Dispose();
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(OperationCanceledException)}]",
                        GuidSession = guidRequest,
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
            _connection.Dispose();
            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = $"[{GetType().Name}]-[{nameof(Exception)}]",
                        GuidSession = guidRequest,
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
                            GuidSession = guidRequest,
                            ReceiverName = queue,
                            PayloadBody = res_io,
                            UTCTimestampInitReceive = DateTime.UtcNow,
                        }, tokenOuter);
                }
                catch
                {

                }

                cts.Cancel();
                //cts.Dispose();
            }, token);
            try
            {
                mres.Wait(token);
            }
            catch (TaskCanceledException ex)
            {
                loggerRepo.LogDebug($"response for {response_topic}");
                _channel.Dispose();
                _connection.Dispose();

                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = $"[{GetType().Name}]-[{nameof(TaskCanceledException)}]",
                            GuidSession = guidRequest,
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
                loggerRepo.LogDebug($"response for {response_topic}");
                _channel.Dispose();
                _connection.Dispose();
                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = $"[{GetType().Name}]-[{nameof(OperationCanceledException)}]",
                            GuidSession = guidRequest,
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
                _connection.Dispose();

                try
                {
                    if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                        await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                        {
                            Sender = $"[{GetType().Name}]-[{nameof(Exception)}]",
                            GuidSession = guidRequest,
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
                msg = $"Elapsed for `{queue}` -> `{response_topic}`: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(RabbitConfigRepo.RemoteCallTimeoutMs)}\n{request_payload_json}";
                loggerRepo.LogError(msg);
                stopwatch.Stop();
            }
            else
                loggerRepo.LogDebug($"Elapsed [{queue}] -> [{response_topic}]: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(RabbitConfigRepo.RemoteCallTimeoutMs)}");
        }
        else
            return default;

        activity?.Stop();

        if (typeof(T) != typeof(object) && (res_io is null || res_io.Response is null))
        {
            msg = $"Response MQ/IO is null [{queue}] -> [{response_topic}]: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(RabbitConfigRepo.RemoteCallTimeoutMs)}";
            loggerRepo.LogError(msg);

            try
            {
                if (ITraceRabbitActionsService.TracesFilter is null || ITraceRabbitActionsService.TracesFilter.Any(x => queue.Contains(x)))
                    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
                    {
                        Sender = typeof(T).Name,
                        GuidSession = guidRequest,
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
                    GuidSession = guidRequest,
                    ReceiverName = queue,
                    PayloadBody = res_io,
                    UTCTimestampInitReceive = DateTime.UtcNow,
                }, tokenOuter);
        }
        catch
        {

        }

        return res_io.Response;
    }
}