﻿////////////////////////////////////////////////
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
    readonly IConnection _connection;
    readonly IModel _channel;
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

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    /// <inheritdoc/>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        TResponseMQModel<TResponse> answer = new()
        {
            StartedServer = DateTime.UtcNow,
        };
        EventingBasicConsumer consumer = new(_channel);
        consumer.Received += async (ch, ea) =>
        {
            TRequest? sr;

#if DEBUG
            string content = Encoding.UTF8.GetString(ea.Body.ToArray()).Trim();
            try
            {
                sr = content.Equals("null", StringComparison.OrdinalIgnoreCase)
                ? default
                : JsonConvert.DeserializeObject<TRequest?>(content);

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
                    _channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: null, body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(answer, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            }
            else
                _channel.BasicAck(ea.DeliveryTag, false);
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
                    _channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: null, body: System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(answer));
                }
                finally
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            }
            else
                _channel.BasicAck(ea.DeliveryTag, false);
#endif
        };
        LoggerRepo.LogTrace($"BasicConsume QueueName:{QueueName};");
        _channel.BasicConsume(QueueName, false, consumer);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}