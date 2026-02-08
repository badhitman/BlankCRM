////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

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

namespace RemoteCallLib;

/// <summary>
/// Удалённый вызов команд (RabbitMq client)
/// </summary>
public class RabbitClient : IRabbitClient
{
    readonly RabbitMQConfigModel RabbitConfigRepo;
    readonly ConnectionFactory factory;
    readonly ILogger<RabbitClient> loggerRepo;

    readonly string AppName;

    static Dictionary<string, object>? ResponseQueueArguments;
    /// <inheritdoc/>
    public static readonly JsonSerializerOptions SerializerOptions = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true };

    /// <summary>
    /// Удалённый вызов команд (RabbitMq client)
    /// </summary>
    public RabbitClient(IOptions<RabbitMQConfigModel> rabbitConf, ILogger<RabbitClient> _loggerRepo, string appName)
    {
        AppName = appName;
        loggerRepo = _loggerRepo;
        RabbitConfigRepo = rabbitConf.Value;
        ResponseQueueArguments ??= new()
        {
            { "x-message-ttl", rabbitConf.Value.RemoteCallTimeoutMs },
            { "x-expires", rabbitConf.Value.RemoteCallTimeoutMs },
            { "x-consumer-timeout", rabbitConf.Value.RemoteCallTimeoutMs + 100 },
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

        string response_topic = waitResponse ? $"{RabbitConfigRepo.QueueMqNamePrefixForResponse.Replace("\\", "/")}{queue}_{Guid.NewGuid()}" : "";
        activity?.SetTag(nameof(response_topic), response_topic);

        using IConnection _connection = await factory.CreateConnectionAsync(tokenOuter);
        using IChannel _channel = await _connection.CreateChannelAsync(cancellationToken: tokenOuter);

        string _msg;
        BasicProperties? properties = new();
        if (waitResponse)
        {
            properties.ReplyTo = response_topic;
            try
            {
                await _channel.QueueDeclareAsync(queue: response_topic, durable: false, exclusive: false, autoDelete: true, arguments: ResponseQueueArguments!, cancellationToken: tokenOuter);
            }
            catch (TaskCanceledException) { }
            catch (Exception ex)
            {
                _msg = "exception basic ask. error {C8C5AB97-CE68-4A5B-BB7D-FA71C6419A3E}";
                loggerRepo.LogError(ex, _msg);
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
            catch (Exception ex)
            {
                msg = $"error deserialisation: {content}.\n\nerror ";
                loggerRepo.LogError(ex, msg);
            }

            try
            {
                await _channel.BasicAckAsync(e.DeliveryTag, false, tokenOuter);
            }
            catch (TaskCanceledException) { }
            catch (Exception ex)
            {
                msg = "exception basic ask. error {A62029D4-1A23-461D-99AD-349C6B7500A8}";
                loggerRepo.LogError(ex, msg);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        await _channel!.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: tokenOuter);

#if DEBUG
        string request_payload_json = "";
        try
        {
            request_payload_json = JsonConvert.SerializeObject(request, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка сериализации объекта [{request?.GetType().Name}]: {request}");
        }

        byte[] body = request is null ? [] : Encoding.UTF8.GetBytes(request_payload_json);
#else
        byte[] body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(request, SerializerOptions);
#endif

        await _channel!.BasicPublishAsync(exchange: "",
                        routingKey: queue,
                        mandatory: true,
                        basicProperties: properties,
                        body: body,
                        cancellationToken: tokenOuter);

        if (waitResponse)
        {
            stopwatch.Start();
            _ = Task.Run(async () =>
            {
                await Task.Delay(RabbitConfigRepo.RemoteCallTimeoutMs);
                cts.Cancel();
            }, tokenOuter);
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

            if (stopwatch.IsRunning)
            {
                _msg = $"Elapsed for `{queue}` -> `{response_topic}`: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(RabbitConfigRepo.RemoteCallTimeoutMs)}";
                loggerRepo.LogError(_msg);
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
            _msg = $"Response MQ/IO is null [{queue}] -> [{response_topic}]: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(RabbitConfigRepo.RemoteCallTimeoutMs)}";
            loggerRepo.LogError(_msg);
            return default;
        }
        else if (res_io is null)
            return default;

        return res_io.Response;
    }
}