////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using SharedLib;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        ResponseQueueArguments ??= new()
        {
            { "x-message-ttl", rabbitConf.Value.RemoteCallTimeoutMs },
            { "x-expires", rabbitConf.Value.RemoteCallTimeoutMs },
            { "x-consumer-timeout", rabbitConf.Value.RemoteCallTimeoutMs + 100 },
            { "x-queue-type", "quorum" },
        };
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
        string guidRequest = Guid.NewGuid().ToString();

        //try
        //{
        //    await traceRepo.SaveActionAsync(new TraceRabbitActionRequestModel()
        //    {
        //        ReceiverName = queue,
        //        RequestBody = request,
        //        UTCTimestampInitReceive = DateTime.UtcNow,
        //    }, tokenOuter);
        //}
        //catch
        //{

        //}

        string response_topic = waitResponse ? $"{AppName}.{RabbitConfigRepo.QueueMqNamePrefixForResponse.Replace("\\", "/")}{queue}_{guidRequest}" : "";
        activity?.SetTag(nameof(response_topic), response_topic);

        string msg;
        IConnection? _connection = null;
        IChannel? _channel = null;

        try
        {
            _connection = await factory.CreateConnectionAsync(tokenOuter);
            _channel = await _connection.CreateChannelAsync(cancellationToken: tokenOuter);
        }
        catch (TaskCanceledException)
        {
            _connection?.Dispose();
            _channel?.Dispose();

            return default;
        }
        catch (OperationCanceledException)
        {
            _connection?.Dispose();
            _channel?.Dispose();

            return default;
        }
        catch (Exception ex)
        {
            msg = "exception basic ask. error {295C630E-4069-44A1-8619-15E418F4BF58}";
            loggerRepo.LogError(ex, msg);

            _connection?.Dispose();
            _channel?.Dispose();

            return default;
        }

        BasicProperties? properties = new();
        if (waitResponse)
        {
            properties.ReplyTo = response_topic;
            try
            {
                await _channel.QueueDeclareAsync(queue: response_topic, durable: false, exclusive: false, autoDelete: true, arguments: new Dictionary<string, object>(ResponseQueueArguments!.Where(x => x.Key != "x-queue-type"))!, cancellationToken: tokenOuter);
            }
            catch (TaskCanceledException)
            {
                _connection.Dispose();
                _channel.Dispose();

                return default;
            }
            catch (OperationCanceledException)
            {
                _connection?.Dispose();
                _channel?.Dispose();

                return default;
            }
            catch (OperationInterruptedException ex)
            {
                msg = $"exception basic ask for [queue: {response_topic}]. error 56AA49DF-E8F8-489F-A2AB-591511EE7B33";
                loggerRepo.LogError(ex, msg);

                _connection?.Dispose();
                _channel?.Dispose();

                return default;
            }
            catch (Exception ex)
            {
                msg = "exception basic ask. error {C8C5AB97-CE68-4A5B-BB7D-FA71C6419A3E}";
                loggerRepo.LogError(ex, msg);

                _connection.Dispose();
                _channel.Dispose();

                return default;
            }
        }

        Stopwatch stopwatch = new();
        AsyncEventingBasicConsumer consumer = new(_channel);

        CancellationTokenSource cts = new();
        CancellationToken token = cts.Token;
        ManualResetEventSlim mres = new(false);
        TResponseMQModel<T?>? res_io = null;
        async Task MessageReceivedEvent(object? sender, BasicDeliverEventArgs e)
        {
            string msg;
            consumer.ReceivedAsync -= MessageReceivedEvent;
            string content = Encoding.UTF8.GetString(e.Body.ToArray());

            if (!content.Contains(GlobalStaticConstantsRoutes.Routes.PASSWORD_CONTROLLER_NAME, StringComparison.OrdinalIgnoreCase))
                activity?.SetBaggage(nameof(content), content);
            else
                activity?.SetBaggage(nameof(content), $"MUTE: `{GlobalStaticConstantsRoutes.Routes.PASSWORD_CONTROLLER_NAME}` - contains");

            try
            {
                res_io = JsonConvert.DeserializeObject<TResponseMQModel<T?>>(content, GlobalStaticConstants.JsonSerializerSettings)
                    ?? throw new Exception("parse error {0CBCCD44-63C8-4E93-8349-11A8BE63B235}");

                if (!res_io.Success())
                    loggerRepo.LogError(res_io.Message());

                countGreetings.Add(res_io.Duration().Milliseconds);
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                msg = $"error deserialisation: {content}.\n\nerror ";
                loggerRepo.LogError(ex, msg);

                return;
            }

            try
            {
                await _channel.BasicAckAsync(e.DeliveryTag, false, tokenOuter);
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                msg = "exception basic ask. error {A62029D4-1A23-461D-99AD-349C6B7500A8}";
                loggerRepo.LogError(ex, msg);

                return;
            }

            stopwatch.Stop();
            cts.Cancel();
            cts.Dispose();
        }

        consumer.ReceivedAsync += MessageReceivedEvent;

        if (waitResponse)
        {
            try
            {
                await _channel.BasicConsumeAsync(response_topic, false, consumer, cancellationToken: tokenOuter);
            }
            catch (TaskCanceledException)
            {
                _connection.Dispose();
                _channel.Dispose();

                return default;
            }
            catch (OperationCanceledException)
            {
                _connection.Dispose();
                _channel.Dispose();

                return default;
            }
            catch (Exception ex)
            {
                loggerRepo.LogError(ex, "exception 88DE88EF-10C7-4BB8-A36A-F9C6DFDA70B2");

                _connection.Dispose();
                _channel.Dispose();

                return default;
            }
        }

        try
        {
            await _channel!.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: ResponseQueueArguments!, cancellationToken: tokenOuter);
        }
        catch (TaskCanceledException)
        {
            _connection.Dispose();
            _channel.Dispose();

            return default;
        }
        catch (OperationCanceledException)
        {
            _connection.Dispose();
            _channel.Dispose();

            return default;
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, "exception 52301C76-8A66-466B-9553-56D44711135A");

            _connection.Dispose();
            _channel.Dispose();

            return default;
        }

#if DEBUG
        string request_payload_json = "";
        try
        {
            request_payload_json = JsonConvert.SerializeObject(request, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка сериализации объекта [{request?.GetType().Name}]: {request}");
            _connection.Dispose();
            _channel.Dispose();

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
        catch (TaskCanceledException)
        {
            _connection.Dispose();
            _channel.Dispose();

            return default;
        }
        catch (OperationCanceledException)
        {
            _connection.Dispose();
            _channel.Dispose();

            return default;
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, "exception 4BDEC834-2CA1-42D6-9A69-9F3700F064C2");
            _connection.Dispose();
            _channel.Dispose();

            return default;
        }

        if (waitResponse)
        {
            //tokenOuter.Register(cts.Cancel);
            stopwatch.Start();
            _ = Task.Run(async () =>
            {
                await Task.Delay(RabbitConfigRepo.RemoteCallTimeoutMs, token);
                cts.Cancel();
                //cts.Dispose();
            }, token);
            try
            {
                mres.Wait(token);
            }
            catch (TaskCanceledException)
            {
                loggerRepo.LogDebug($"response for {response_topic}");
                _connection.Dispose();
                _channel.Dispose();
            }
            catch (OperationCanceledException)
            {
                loggerRepo.LogDebug($"response for {response_topic}");
                _connection.Dispose();
                _channel.Dispose();
            }
            catch (Exception ex)
            {
                msg = "exception Wait response. error {8B621451-2214-467F-B8E9-906DD866662C}";
                loggerRepo.LogError(ex, msg);
                stopwatch.Stop();

                _connection.Dispose();
                _channel.Dispose();

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
            return default;
        }
        else if (res_io is null)
            return default;

        return res_io.Response;
    }
}