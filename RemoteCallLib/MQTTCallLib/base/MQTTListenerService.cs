////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using SharedLib;
using MQTTnet;
using System;

namespace RemoteCallLib;

/// <summary>
/// Обработчик входящих запросов
/// </summary>
/// <typeparam name="TQueue">Очередь</typeparam>
/// <typeparam name="TRequest">Запрос</typeparam>
/// <typeparam name="TResponse">Ответ</typeparam>
public class MQTTListenerService<TQueue, TRequest, TResponse>
    : BackgroundService
    where TQueue : IMQReceive<TRequest?, TResponse>
    where TResponse : new()
{
    readonly RealtimeMQTTClientConfigModel MQConfigRepo;
    readonly ILogger<MQTTListenerService<TQueue, TRequest, TResponse>> LoggerRepo;
    readonly IMQReceive<TRequest?, TResponse> receiveService;

    IMqttClient mqttClient;
    MqttClientFactory mqttFactoryCLI = new();

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
    public MQTTListenerService(
        RealtimeMQTTClientConfigModel rabbitConf,
        IServiceProvider servicesProvider,
        ILogger<MQTTListenerService<TQueue, TRequest, TResponse>> loggerRepo)
    {
        LoggerRepo = loggerRepo;
        MQConfigRepo = rabbitConf;

        using IServiceScope scope = servicesProvider.CreateScope();
        receiveService = scope.ServiceProvider.GetServices<IMQReceive<TRequest?, TResponse>>().First(o => o.GetType() == QueueType);
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
                e.ReasonCode = MqttApplicationMessageReceivedReasonCode.Success;
                e.ResponseReasonString = typeof(TRequest).Name;
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
                await mqttClient.PublishAsync(applicationMessage, stoppingToken);
            }
        };

        await mqttClient.ConnectAsync(GetMqttClientOptionsBuilder(new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1])), stoppingToken);
        await mqttClient.SubscribeAsync(QueueName, cancellationToken: stoppingToken);
    }
    MqttClientOptions GetMqttClientOptionsBuilder(KeyValuePair<string, byte[]>? propertyValue)
    {
        MqttClientOptionsBuilder res = new MqttClientOptionsBuilder()
               .WithTcpServer(MQConfigRepo.Host, MQConfigRepo.Port)
               .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
               .WithClientId($"{QueueName} [{nameof(MQTTListenerService<,,>)}.{typeof(TRequest).Name}] {Guid.NewGuid()}");

        if (propertyValue is not null)
            res.WithUserProperty(propertyValue.Value.Key, propertyValue.Value.Value);

        return res.Build();
    }
    /// <inheritdoc/>
    public override void Dispose()
    {
        mqttClient.DisconnectAsync(new() { UserProperties = [new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, new ReadOnlyMemory<byte>([1]))] });
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}