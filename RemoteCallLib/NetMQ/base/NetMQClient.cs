////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using Newtonsoft.Json;
using NetMQ.Sockets;
using SharedLib;
using NetMQ;

namespace RemoteCallLib;

/// <summary>
/// Удалённый вызов команд (NetMq client)
/// </summary>
/// <inheritdoc/>
public class NetMQClient(IOptions<ProxyNetMQConfigModel> mqConf, ILogger<NetMQClient> _loggerRepo, string appName) : IMQClientRPC
{
    readonly ProxyNetMQConfigModel MQConfigRepo = mqConf.Value;
    readonly PublisherSocket pubSocket = new();
    readonly ILogger<NetMQClient> loggerRepo = _loggerRepo;
    readonly string AppName = appName;

    /// <inheritdoc/>
    public async Task<T?> MqRemoteCallAsync<T>(string queue, object? request = null, bool waitResponse = true, CancellationToken tokenOuter = default)
    {
        queue = queue.Replace("\\", "/");
        pubSocket.Connect(MQConfigRepo.PublisherSocketEndpoint.ToString());
        pubSocket.Options.SendHighWatermark = 1000;

        Meter greeterMeter = new($"OTel.{AppName}", "1.0.0");
        Counter<long> countGreetings = greeterMeter.CreateCounter<long>(GlobalStaticConstantsRoutes.Routes.DURATION_ACTION_NAME, description: "Длительность в мс.");

        Stopwatch stopwatch = new();
        CancellationTokenSource cts = new();
        CancellationToken token = cts.Token;
        ManualResetEventSlim mres = new(false);
        string _msg;
        TResponseMQModel<T?>? res_io = null;
        string request_payload_json = "";
        try
        {
            request_payload_json = JsonConvert.SerializeObject(request, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings);
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, $"Ошибка сериализации объекта [{request?.GetType().Name}]: {request}");
        }

        string response_topic = waitResponse ? $"{MQConfigRepo.QueueMqNamePrefixForResponse}{queue}_{Guid.NewGuid()}" : "";

        async Task ResponseClient_ApplicationMessageReceivedAsync(List<string> eMsg)
        {
            string msg;
            string content = string.Join("", eMsg);

            try
            {
                res_io = JsonConvert.DeserializeObject<TResponseMQModel<T?>>(content, GlobalStaticConstants.JsonSerializerSettings)
                    ?? throw new Exception("parse error {03D3DB2E-5682-4D70-8EF7-278B8B8A9D44}");

                if (!res_io.Success())
                    loggerRepo.LogError(res_io.Message());

                countGreetings.Add(res_io.Duration().Milliseconds);
            }
            catch (Exception ex)
            {
                msg = $"error deserialisation: {content}.\n\nerror ";
                loggerRepo.LogError(ex, msg);
            }

            stopwatch.Stop();
            cts.Cancel();
            cts.Dispose();
        }

        loggerRepo.LogTrace($"Sending message into queue [{queue}]", request_payload_json);

        using SubscriberSocket subSocket = new();
        if (waitResponse)
        {
            stopwatch.Start();
            try
            {
                subSocket.Connect(MQConfigRepo.SubscriberSocketEndpoint.ToString());
                subSocket.Options.ReceiveHighWatermark = 1000;
                subSocket.Subscribe(response_topic);

                pubSocket.SendMoreFrame(response_topic).SendFrame(request_payload_json);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _msg = $"Request MQ/IO error [{queue}]";
                loggerRepo.LogError(ex, _msg);

                return default;
            }

            List<string> resMsg = [];
            List<Task> tasks = [
                Task.Run(async () => { await Task.Delay(MQConfigRepo.RemoteCallTimeoutMs, token); cts.Cancel(); }, tokenOuter),
                Task.Run(async () =>
                {
                    (string, bool) messageTopicReceived = await subSocket.ReceiveFrameStringAsync(token);
                    resMsg.Add(messageTopicReceived.Item1);
                    while(messageTopicReceived.Item2)
                    {
                        messageTopicReceived = await subSocket.ReceiveFrameStringAsync(token);
                        resMsg.Add(messageTopicReceived.Item1);
                    }
                    // string messageReceived = subSocket.ReceiveString();           
                    cts.Cancel();
                }, tokenOuter),
                Task.Run(() => {
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
                }, tokenOuter)];

            await Task.WhenAny(tasks);
            if (resMsg.Count != 0)
                await ResponseClient_ApplicationMessageReceivedAsync(resMsg);

            if (stopwatch.IsRunning)
            {
                _msg = $"Elapsed for `{queue}`: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(MQConfigRepo.RemoteCallTimeoutMs)}";
                loggerRepo.LogError(_msg);
                stopwatch.Stop();
            }
            else
                loggerRepo.LogDebug($"Elapsed [{queue}]: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(MQConfigRepo.RemoteCallTimeoutMs)}");
        }
        else
        {
            pubSocket.SendFrame(request_payload_json);
            return default;
        }

        if (typeof(T) != typeof(object) && (res_io is null || res_io.Response is null))
        {
            _msg = $"Response MQ/IO is null [{queue}]: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(MQConfigRepo.RemoteCallTimeoutMs)}";
            loggerRepo.LogError(_msg);
            return default;
        }
        else if (res_io is null)
            return default;

        return res_io.Response;
    }
}