////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NetMQ.Sockets;
using SharedLib;
using NetMQ;

namespace RemoteCallLib;

/// <summary>
/// Обработчик входящих запросов
/// </summary>
/// <typeparam name="TQueue">Очередь</typeparam>
/// <typeparam name="TRequest">Запрос</typeparam>
/// <typeparam name="TResponse">Ответ</typeparam>
public class NetMQListenerService<TQueue, TRequest, TResponse>
    : BackgroundService
    where TQueue : IMQStandardReceive<TRequest?, TResponse>
    where TResponse : new()
{
    readonly IOptions<ProxyNetMQConfigModel> MQConfigRepo;
    readonly ILogger<NetMQListenerService<TQueue, TRequest, TResponse>> LoggerRepo;
    readonly IMQStandardReceive<TRequest?, TResponse> receiveService;

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
            _queueName ??= QueueType.GetProperties().First(x => x.Name.Equals(nameof(IBaseStandardReceive.QueueName))).GetValue(null)!.ToString()!.Replace("\\", "/");
            return _queueName;
        }
    }

    /// <inheritdoc/>
    public NetMQListenerService(
        IOptions<ProxyNetMQConfigModel> mqConf,
        IServiceProvider servicesProvider,
        ILogger<NetMQListenerService<TQueue, TRequest, TResponse>> loggerRepo)
    {
        LoggerRepo = loggerRepo;
        MQConfigRepo = mqConf;

        using IServiceScope scope = servicesProvider.CreateScope();
        receiveService = scope.ServiceProvider.GetServices<IMQStandardReceive<TRequest?, TResponse>>().First(o => o.GetType() == QueueType);
        subSocket = new();

        loggerRepo.LogInformation($"Subscriber [{QueueName}] socket ready...");
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        TResponseMQModel<TResponse> answer = new() { StartedServer = DateTime.UtcNow, };

        subSocket.Connect(MQConfigRepo.Value.SubscriberSocketEndpoint.ToString());
        subSocket.Options.ReceiveHighWatermark = 1000;
        subSocket.Subscribe(QueueName);
        Console.WriteLine("Subscriber socket connecting...");
        using NetMQRuntime runtime = new();
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
        }
    }
    /// <inheritdoc/>
    public override void Dispose()
    {
        subSocket.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}