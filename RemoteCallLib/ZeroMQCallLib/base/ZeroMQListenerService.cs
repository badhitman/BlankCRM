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
#if !DEBUG
using System.Text.Json.Serialization;
using System.Text.Json;
#endif

namespace RemoteCallLib
{
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
        readonly StockSharpClientConfig MQConfigRepo;
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
        public ZeroMQListenerService(IOptions<StockSharpClientConfig> rabbitConf, IServiceProvider servicesProvider, ILogger<ZeroMQListenerService<TQueue, TRequest, TResponse>> loggerRepo)
        {
            LoggerRepo = loggerRepo;
            MQConfigRepo = rabbitConf.Value;

            using IServiceScope scope = servicesProvider.CreateScope();
            receiveService = scope.ServiceProvider.GetServices<IZeroMQReceive<TRequest?, TResponse>>().First(o => o.GetType() == QueueType);


            subSocket = new SubscriberSocket();
            subSocket.Options.ReceiveHighWatermark = 1000;
            subSocket.Connect(MQConfigRepo.ToString());
            subSocket.Subscribe(QueueName);
            Console.WriteLine("Subscriber socket connecting...");
            //while (true)
            //{
            //    string messageTopicReceived = subSocket.ReceiveFrameString();
            //    string messageReceived = subSocket.ReceiveFrameString();
            //    Console.WriteLine(messageReceived);
            //}

            //    factory = new()
            //    {
            //        ClientProvidedName = rabbitConf.Value.ClientProvidedName,
            //        HostName = rabbitConf.Value.HostName,
            //        UserName = rabbitConf.Value.UserName,
            //        Password = rabbitConf.Value.Password
            //    };

            //    _connection = factory.CreateConnection();
            //    _channel = _connection.CreateModel();
            //    _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            TResponseMQModel<TResponse> answer = new()
            {
                StartedServer = DateTime.UtcNow,
            };

            //            consumer.Received += async (ch, ea) =>
            //            {
            //                TRequest? sr;

            //#if DEBUG
            //                string content = Encoding.UTF8.GetString(ea.Body.ToArray()).Trim();
            //                try
            //                {
            //                    sr = content.Equals("null", StringComparison.OrdinalIgnoreCase)
            //                    ? default
            //                    : JsonConvert.DeserializeObject<TRequest?>(content);

            //                    answer.Response = await receiveService.ResponseHandleActionAsync(sr);
            //                }
            //                catch (Exception ex)
            //                {
            //                    answer.Messages.InjectException(ex);
            //                    LoggerRepo.LogError(ex, $"Ошибка выполнения удалённой команды: {QueueName}");
            //                }

            //                answer.FinalizedServer = DateTime.UtcNow;

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
            //#else
            //            try
            //            {
            //                sr = System.Text.Json.JsonSerializer.Deserialize<TRequest?>(ea.Body.ToArray());
            //                answer.Response = await receiveService.ResponseHandleActionAsync(sr);
            //            }
            //            catch (Exception ex)
            //            {
            //                answer.Messages.InjectException(ex);
            //                LoggerRepo.LogError(ex, $"Ошибка выполнения удалённой команды: {QueueName}");
            //            }
            //            answer.FinalizedServer = DateTime.UtcNow;
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
            //#endif
            //            };

            //            _channel.BasicConsume(QueueName, false, consumer);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            subSocket.Close();
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}