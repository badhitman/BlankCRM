////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Hosting;
using SharedLib;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Newtonsoft.Json;
using MQTTnet;
using System.Text;
using MQTTnet.Server;

namespace RemoteCallLib;

/// <summary>
/// Обработчик входящих запросов
/// </summary>
/// <typeparam name="TQueue">Очередь</typeparam>
/// <typeparam name="TRequest">Запрос</typeparam>
/// <typeparam name="TResponse">Ответ</typeparam>
public class MQTTListenerService<TQueue, TRequest, TResponse>
    : BackgroundService
    where TQueue : IMQTTReceive<TRequest?, TResponse>
    where TResponse : new()
{
    readonly StockSharpClientConfigModel MQConfigRepo;
    readonly ILogger<MQTTListenerService<TQueue, TRequest, TResponse>> LoggerRepo;
    readonly IMQTTReceive<TRequest?, TResponse> receiveService;

    IMqttClient mqttClient;
    MqttClientFactory mqttFactoryCLI = new ();
    MqttServerFactory mqttFactory = new();

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
            _queueName ??= QueueType.GetProperties().First(x => x.Name.Equals(nameof(IBaseReceive.QueueName))).GetValue(null)!.ToString()!;
            return _queueName;
        }
    }

    /// <inheritdoc/>
    public MQTTListenerService(
        StockSharpClientConfigModel rabbitConf,
        IServiceProvider servicesProvider,
        ILogger<MQTTListenerService<TQueue, TRequest, TResponse>> loggerRepo)
    {
        LoggerRepo = loggerRepo;
        MQConfigRepo = rabbitConf;

        using IServiceScope scope = servicesProvider.CreateScope();
        receiveService = scope.ServiceProvider.GetServices<IMQTTReceive<TRequest?, TResponse>>().First(o => o.GetType() == QueueType);
        mqttClient = mqttFactoryCLI.CreateMqttClient();
        loggerRepo.LogInformation($"Subscriber [{QueueName}] socket ready...");
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        TResponseMQModel<TResponse> answer = new() { StartedServer = DateTime.UtcNow, };

        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            TRequest? sr;
            string content = Encoding.UTF8.GetString(e.ApplicationMessage.Payload).Trim();
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
                //
                e.ReasonCode = MqttApplicationMessageReceivedReasonCode.UnspecifiedError;
                e.ResponseReasonString = ex.Message;
            }
            
            answer.FinalizedServer = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(e.ApplicationMessage.ResponseTopic))
            {
                using IMqttClient mqttResponseClient = mqttFactoryCLI.CreateMqttClient();
                await mqttResponseClient.ConnectAsync(GetMqttClientOptionsBuilder, stoppingToken);

                MqttApplicationMessage applicationMessage;
                try
                {
                    applicationMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(e.ApplicationMessage.ResponseTopic)
                        .WithPayload(JsonConvert.SerializeObject(answer, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings))
                        .Build();
                }
                catch (Exception ex)
                {
                    answer.Response = default;
                    LoggerRepo.LogError(ex, $"Ошибка `{ex.GetType().Name}` отправки ответа для топика {QueueName}");
                    answer.Messages.InjectException(ex);
                   
                    applicationMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(e.ApplicationMessage.ResponseTopic)
                        .WithPayload(JsonConvert.SerializeObject(answer, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings))
                        .Build();
                }
                await mqttResponseClient.PublishAsync(applicationMessage, stoppingToken);
            }
        };

        await mqttClient.ConnectAsync(GetMqttClientOptionsBuilder, stoppingToken);
        await mqttClient.SubscribeAsync(QueueName, cancellationToken: stoppingToken);
    }
    MqttClientOptions GetMqttClientOptionsBuilder
    {
        get
        {
            return new MqttClientOptionsBuilder()
               .WithTcpServer(MQConfigRepo.Host, MQConfigRepo.Port)
               .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
               .Build();
        }
    }
    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}