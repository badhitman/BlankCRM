////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using SharedLib;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteCallLib;

/// <summary>
/// Обработчик входящих запросов
/// </summary>
/// <typeparam name="TQueue">Очередь</typeparam>
/// <typeparam name="TRequest">Запрос</typeparam>
/// <typeparam name="TResponse">Ответ</typeparam>
public class NetMQListenerService<TQueue, TRequest, TResponse>
    : BackgroundService
    where TQueue : IMQReceive<TRequest?, TResponse>
    where TResponse : new()
{
    readonly IOptions<ProxyNetMQConfigModel> MQConfigRepo;
    readonly ILogger<NetMQListenerService<TQueue, TRequest, TResponse>> LoggerRepo;
    readonly IMQReceive<TRequest?, TResponse> receiveService;

    SubscriberSocket subSocket;
    //IMqttClient mqttClient;
    //MqttClientFactory mqttFactoryCLI = new();

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
            _queueName ??= QueueType.GetProperties().First(x => x.Name.Equals(nameof(IBaseReceive.QueueName))).GetValue(null)!.ToString()!.Replace("\\", "/");
            return _queueName;
        }
    }

    /// <inheritdoc/>
    public NetMQListenerService(
        IOptions<ProxyNetMQConfigModel> rabbitConf,
        IServiceProvider servicesProvider,
        ILogger<NetMQListenerService<TQueue, TRequest, TResponse>> loggerRepo)
    {
        LoggerRepo = loggerRepo;
        MQConfigRepo = rabbitConf;

        using IServiceScope scope = servicesProvider.CreateScope();
        receiveService = scope.ServiceProvider.GetServices<IMQReceive<TRequest?, TResponse>>().First(o => o.GetType() == QueueType);
        subSocket = new();

        loggerRepo.LogInformation($"Subscriber [{QueueName}] socket ready...");
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        TResponseMQModel<TResponse> answer = new() { StartedServer = DateTime.UtcNow, };

        //using SubscriberSocket subSocket = new();
        subSocket.Connect("tcp://127.0.0.1:1234");
        subSocket.Options.ReceiveHighWatermark = 1000;
        subSocket.Subscribe(QueueName);
        Console.WriteLine("Subscriber socket connecting...");

        while (!stoppingToken.IsCancellationRequested)
        {
            List<string> _rawParts = [];
            // Содержимое полученного кадра сообщения в виде строки и логического значения указывает, следует ли за ним другой кадр того же сообщения.
            (string, bool) messageTopicReceived = await subSocket.ReceiveFrameStringAsync(cancellationToken: stoppingToken);
            _rawParts.Add(messageTopicReceived.Item1);
            while (messageTopicReceived.Item2)
            {
                messageTopicReceived = await subSocket.ReceiveFrameStringAsync(cancellationToken: stoppingToken);
                _rawParts.Add(messageTopicReceived.Item1);
            }

            //string messageReceived = subSocket.ReceiveFrameString();
            //Console.WriteLine(messageReceived);
            if (_rawParts.Count != 0)
                await ApplicationMessageReceivedAsync(_rawParts);
        }


        async Task ApplicationMessageReceivedAsync(List<string> _msgParts)
        {
            TRequest? sr;
            string content = string.Join("", _msgParts.Skip(1));
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
            if (!string.IsNullOrWhiteSpace(_msgParts[0]))
            {
                //            using IMqttClient mqttResponseClient = mqttFactoryCLI.CreateMqttClient();
                //            await mqttResponseClient.ConnectAsync(GetMqttClientOptionsBuilder(new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1])), stoppingToken);

                //            MqttApplicationMessage applicationMessage;
                //            try
                //            {
                //                applicationMessage = new MqttApplicationMessageBuilder()
                //                    .WithTopic(_msgParts[0])
                //                    .WithPayload(JsonConvert.SerializeObject(answer, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings))
                //                    .Build();
                //            }
                //            catch (Exception ex)
                //            {
                //                answer.Response = default;
                //                LoggerRepo.LogError(ex, $"Ошибка `{ex.GetType().Name}` отправки ответа для топика {QueueName}");
                //                answer.Messages.InjectException(ex);

                //                applicationMessage = new MqttApplicationMessageBuilder()
                //                    .WithTopic(_msgParts[0])
                //                    .WithPayload(JsonConvert.SerializeObject(answer, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings))
                //                    .Build();
                //            }
                //            await mqttResponseClient.PublishAsync(applicationMessage, stoppingToken);
                //        }
            }
            ;

            //    await mqttClient.ConnectAsync(GetMqttClientOptionsBuilder(new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1])), stoppingToken);
            //    await mqttClient.SubscribeAsync(QueueName, cancellationToken: stoppingToken);
        }
    }
    /// <inheritdoc/>
    public override void Dispose()
    {
        //    mqttClient.DisconnectAsync(new() { UserProperties = [new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, new ReadOnlyMemory<byte>([1]))] });
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}