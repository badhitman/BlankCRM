////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SharedLib;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using NetMQ.Sockets;
using NetMQ;
using Newtonsoft.Json;
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
public class ZeroMQListenerService<TQueue, TRequest, TResponse>
    : BackgroundService
    where TQueue : IZeroMQReceive<TRequest?, TResponse>
    where TResponse : new()
{
    readonly StockSharpClientConfigModel MQConfigRepo;
    readonly ILogger<ZeroMQListenerService<TQueue, TRequest, TResponse>> LoggerRepo;
    readonly IZeroMQReceive<TRequest?, TResponse> receiveService;
    readonly SubscriberSocket subSocket;

    Type? _queueType;
    /// <summary>
    /// Queue type
    /// </summary>
    Type QueueType { get { _queueType ??= typeof(TQueue); return _queueType; } }

    string? _queueName;
    /// <summary>
    /// Имя очереди MQ
    /// </summary>
    public string QueueName
    {
        get
        {
            _queueName ??= QueueType.GetProperties(System.Reflection.BindingFlags.Static).First(x => x.Name.Equals(nameof(IBaseReceive.QueueName))).GetValue(null, null).ToString();
            return _queueName;
        }
    }

    /// <inheritdoc/>
    public ZeroMQListenerService(StockSharpClientConfigModel rabbitConf, IServiceProvider servicesProvider, ILogger<ZeroMQListenerService<TQueue, TRequest, TResponse>> loggerRepo)
    {
        LoggerRepo = loggerRepo;
        MQConfigRepo = rabbitConf;

        using IServiceScope scope = servicesProvider.CreateScope();
        receiveService = scope.ServiceProvider.GetServices<IZeroMQReceive<TRequest?, TResponse>>().First(o => o.GetType() == QueueType);

        subSocket = new SubscriberSocket();
        subSocket.Options.ReceiveHighWatermark = 1000;
        subSocket.Connect(MQConfigRepo.ToString());
        subSocket.Subscribe(QueueName);
        Console.WriteLine("Subscriber socket connecting...");
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        TResponseMQModel<TResponse> answer = new()
        {
            StartedServer = DateTime.UtcNow,
        };
        while (!stoppingToken.IsCancellationRequested)
        {
            string messageTopicReceived = subSocket.ReceiveFrameString();
            string messageReceived = subSocket.ReceiveFrameString();
            Console.WriteLine(messageReceived);

            TRequest? sr;
#if DEBUG

            try
            {
                sr = messageReceived.Equals("null", StringComparison.OrdinalIgnoreCase)
                ? default
                : JsonConvert.DeserializeObject<TRequest?>(messageReceived);

                answer.Response = await receiveService.ResponseHandleActionAsync(sr);
            }
            catch (Exception ex)
            {
                answer.Messages.InjectException(ex);
                LoggerRepo.LogError(ex, $"Ошибка выполнения удалённой команды: {QueueName}");
            }

            answer.FinalizedServer = DateTime.UtcNow;

            //                if (!string.IsNullOrWhiteSpace(ea.BasicProperties.ReplyTo))
            //                {
            //                    try
            //                    {
            //                        _channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: null, body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(answer, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)));
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        Console.WriteLine(ex);
            //                    }
            //                    finally
            //                    {
            //                        _channel.BasicAck(ea.DeliveryTag, false);
            //                    }
            //                }
            //                else
            //                    _channel.BasicAck(ea.DeliveryTag, false);
#else
                        try
                        {
            //                sr = System.Text.Json.JsonSerializer.Deserialize<TRequest?>(ea.Body.ToArray());
            //                answer.Response = await receiveService.ResponseHandleActionAsync(sr);
                        }
                        catch (Exception ex)
                        {
            //                answer.Messages.InjectException(ex);
            //                LoggerRepo.LogError(ex, $"Ошибка выполнения удалённой команды: {QueueName}");
                        }
                        answer.FinalizedServer = DateTime.UtcNow;
            //            if (!string.IsNullOrWhiteSpace(ea.BasicProperties.ReplyTo))
            //            {
            //                try
            //                {
            //                    _channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: null, body: System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(answer));
            //                }
            //                finally
            //                {
            //                    _channel.BasicAck(ea.DeliveryTag, false);
            //                }
            //            }
            //            else
            //                _channel.BasicAck(ea.DeliveryTag, false);
#endif


            //            _channel.BasicConsume(QueueName, false, consumer);
        }

        subSocket.Close();
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}