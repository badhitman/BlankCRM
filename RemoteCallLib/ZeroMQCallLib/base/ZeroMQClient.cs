////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using Newtonsoft.Json;
using NetMQ.Sockets;
using System.Text;
using System;

namespace SharedLib;

/// <inheritdoc/>
public class ZeroMQClient : IZeroMQClient
{
    readonly StockSharpClientConfigModel MQConfigRepo;
    readonly RequestSocket client;
    readonly ILogger<ZeroMQClient> loggerRepo;

    readonly string AppName;

    /// <inheritdoc/>
    public static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true };

    /// <summary>
    /// Удалённый вызов команд (RabbitMq client)
    /// </summary>
    public ZeroMQClient(StockSharpClientConfigModel mqConf, ILogger<ZeroMQClient> _loggerRepo, string appName)
    {
        AppName = appName;
        loggerRepo = _loggerRepo;
        MQConfigRepo = mqConf;
        client = new RequestSocket();
        client.Connect(MQConfigRepo.ToString());
        //client.SendFrame("Hello");
        //string msg = client.ReceiveFrameString();
        //Console.WriteLine("From Server: {0}", msg);
    }

    /// <inheritdoc/>
    public Task<T?> MqRemoteCallAsync<T>(string queue, object? request = null, bool waitResponse = true, CancellationToken tokenOuter = default) where T : class
    {
        // Custom ActivitySource for the application
        ActivitySource greeterActivitySource = new ActivitySource($"OTel.{AppName}");
        // Create a new Activity scoped to the method
        using Activity? activity = greeterActivitySource.StartActivity($"OTel.{queue}");

        Meter greeterMeter = new Meter($"OTel.{AppName}", "1.0.0");
        Counter<long> countGreetings = greeterMeter.CreateCounter<long>(GlobalStaticConstantsRoutes.Routes.DURATION_ACTION_NAME, description: "Длительность в мс.");
        activity?.Start();
        //activity?.SetTag(nameof(response_topic), response_topic);

        Stopwatch stopwatch = new Stopwatch();

        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        ManualResetEventSlim mres = new ManualResetEventSlim(false);
        string _msg;
        TResponseMQModel<T?>? res_io = null;
        void MessageReceivedEvent(object? sender)
        {
            string msg;
            //consumer.Received -= MessageReceivedEvent;
            //string content = Encoding.UTF8.GetString(e.Body.ToArray());

            //if (!content.Contains(GlobalStaticConstantsRoutes.Routes.PASSWORD_CONTROLLER_NAME, StringComparison.OrdinalIgnoreCase))
            //    activity?.SetBaggage(nameof(content), content);
            //else
            //    activity?.SetBaggage(nameof(content), $"MUTE: `{GlobalStaticConstantsRoutes.Routes.PASSWORD_CONTROLLER_NAME}` - contains");

            //try
            //{
            //    res_io = JsonConvert.DeserializeObject<TResponseMQModel<T?>>(content, GlobalStaticConstants.JsonSerializerSettings)
            //        ?? throw new Exception("parse error {0CBCCD44-63C8-4E93-8349-11A8BE63B235}");

            //    if (!res_io.Success())
            //        loggerRepo.LogError(res_io.Message());

            //    countGreetings.Add(res_io.Duration().Milliseconds);
            //}
            //catch (Exception ex)
            //{
            //    msg = $"error deserialisation: {content}.\n\nerror ";
            //    loggerRepo.LogError(ex, msg);
            //}

            //try
            //{
            //    _channel.BasicAck(e.DeliveryTag, false);
            //}
            //catch (Exception ex)
            //{
            //    msg = "exception basic ask. error {A62029D4-1A23-461D-99AD-349C6B7500A8}";
            //    loggerRepo.LogError(ex, msg);
            //}

            //stopwatch.Stop();
            //cts.Cancel();
            //cts.Dispose();
        }

        //consumer.Received += MessageReceivedEvent;

        if (waitResponse)
        {
            try
            {
                //_channel.BasicConsume(response_topic, false, consumer);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
            }
        }

        activity?.Stop();


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

        byte[] body = request is null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(request_payload_json);
#else
            byte[] body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(request, SerializerOptions);
#endif

        if (waitResponse)
        {
            stopwatch.Start();
            _ = Task.Run(async () =>
            {
                await Task.Delay(MQConfigRepo.RemoteCallTimeoutMs);
                cts.Cancel();
            }, tokenOuter);
            try
            {
                mres.Wait(token);
            }
            catch (OperationCanceledException)
            {
                loggerRepo.LogDebug($"response for `{queue} canceled by timeout");
            }
            catch (Exception ex)
            {
                _msg = "exception Wait response. error {8B621451-2214-467F-B8E9-906DD866662C}";
                loggerRepo.LogError(ex, _msg);
                stopwatch.Stop();
            }

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
            return Task.FromResult(default(T));

        if ((typeof(T) != typeof(object) && (res_io is null || res_io.Response is null)))
        {
            _msg = $"Response MQ/IO is null [{queue}]: {stopwatch.Elapsed} > {TimeSpan.FromMilliseconds(MQConfigRepo.RemoteCallTimeoutMs)}";
            loggerRepo.LogError(_msg);
            return Task.FromResult(default(T));
        }
        else if (res_io is null)
            return Task.FromResult(default(T));

        return Task.FromResult(res_io.Response);
        throw new Exception();
    }
}