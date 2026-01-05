////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using SharedLib;
using Microsoft.Extensions.Logging;
#if !DEBUG
using System.Text.Json.Serialization;
using System.Text.Json;
#endif

namespace RemoteCallLib;

/// <summary>
/// Обработчик входящих запросов
/// </summary>
/// <typeparam name="TQueue">Очередь</typeparam>
/// <typeparam name="TRequest">Запрос</typeparam>
/// <typeparam name="TResponse">Ответ</typeparam>
public class RabbitMqListenerService<TQueue, TRequest, TResponse>
    : BackgroundService
    where TQueue : IResponseReceive<TRequest?, TResponse>
    where TResponse : new()
{
    readonly ILogger<RabbitMqListenerService<TQueue, TRequest, TResponse>> LoggerRepo;
    IConnection _connection = default!;
    IChannel _channel = default!;
    readonly IResponseReceive<TRequest?, TResponse> receiveService;
    readonly ConnectionFactory factory;

    static Dictionary<string, object>? ResponseQueueArguments;

    Type? _queueType;
    /// <summary>
    /// Queue type
    /// </summary>
    Type QueueType { get { _queueType ??= typeof(TQueue); return _queueType; } }

    string? _queueName;
    /// <summary>
    /// Имя очереди MQ
    /// </summary>
    public string QueueName { get { _queueName ??= TQueue.QueueName; return _queueName; } }

    /// <inheritdoc/>
    public RabbitMqListenerService(IOptions<RabbitMQConfigModel> rabbitConf, IServiceProvider servicesProvider, ILogger<RabbitMqListenerService<TQueue, TRequest, TResponse>> loggerRepo)
    {
        LoggerRepo = loggerRepo;
        ResponseQueueArguments ??= new()
        {
            { "x-message-ttl", rabbitConf.Value.RemoteCallTimeoutMs },
            { "x-expires", rabbitConf.Value.RemoteCallTimeoutMs }
        };

        using IServiceScope scope = servicesProvider.CreateScope();
        receiveService = scope.ServiceProvider.GetServices<IResponseReceive<TRequest?, TResponse>>().First(o => o.GetType() == QueueType);
        LoggerRepo.LogTrace($"factory: host:{rabbitConf.Value.HostName}; username:{rabbitConf.Value.UserName};");
        factory = new()
        {
            ClientProvidedName = rabbitConf.Value.ClientProvidedName,
            HostName = rabbitConf.Value.HostName,
            UserName = rabbitConf.Value.UserName,
            Password = rabbitConf.Value.Password
        };
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);

        stoppingToken.ThrowIfCancellationRequested();
        TResponseMQModel<TResponse> answer = new()
        {
            StartedServer = DateTime.UtcNow,
        };
        AsyncEventingBasicConsumer consumer = new(_channel);
        consumer.ReceivedAsync += async (ch, ea) =>
        {
            TRequest? sr;

#if DEBUG
            string content = Encoding.UTF8.GetString(ea.Body.ToArray()).Trim();
            try
            {
                sr = content.Equals("null", StringComparison.OrdinalIgnoreCase)
                ? default
                : JsonConvert.DeserializeObject<TRequest?>(content);

                answer.Response = await receiveService.ResponseHandleActionAsync(sr, stoppingToken);
            }
            catch (Exception ex)
            {
                answer.Messages.InjectException(ex);
                LoggerRepo.LogError(ex, $"Ошибка выполнения удалённой команды: {QueueName}");
            }

            answer.FinalizedServer = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(ea.BasicProperties.ReplyTo))
            {
                try
                {
                    string jsonRawAnswer = JsonConvert.SerializeObject(answer, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);
                    await _channel.BasicPublishAsync(exchange: "", routingKey: ea.BasicProperties.ReplyTo, mandatory: true, body: Encoding.UTF8.GetBytes(jsonRawAnswer), cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken: stoppingToken);
                }
            }
            else
                await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken: stoppingToken);
#else
            try
            {
                sr = System.Text.Json.JsonSerializer.Deserialize<TRequest?>(ea.Body.ToArray());
                answer.Response = await receiveService.ResponseHandleActionAsync(sr);
            }
            catch (Exception ex)
            {
                answer.Messages.InjectException(ex);
                LoggerRepo.LogError(ex, $"Ошибка выполнения удалённой команды: {QueueName}");
            }
            answer.FinalizedServer = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(ea.BasicProperties.ReplyTo))
            {
                try
                {
                    await _channel.BasicPublishAsync(exchange: "", routingKey: ea.BasicProperties.ReplyTo, mandatory: true, body: System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(answer), cancellationToken: stoppingToken);
                }
                finally
                {
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
            }
            else
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
#endif
        };
        LoggerRepo.LogTrace($"BasicConsume QueueName:{QueueName};");
        await _channel.BasicConsumeAsync(QueueName, false, consumer, cancellationToken: stoppingToken);
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        //_channel.Close();
        //_connection.Close();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}