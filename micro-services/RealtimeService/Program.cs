////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;
using NLog.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using MQTTnet.AspNetCore;
using Newtonsoft.Json;
using MQTTnet.Server;
using OpenTelemetry;
using RemoteCallLib;
using ServicesLib;
using System.Text;
using SharedLib;
using DbcLib;
using NLog;

namespace RealtimeService;

public class Program
{
    static Logger logger = LogManager.Setup().LoadConfigurationFromFile().GetCurrentClassLogger();
    static readonly RealtimeMQTTClientConfigModel _confMQTT = RealtimeMQTTClientConfigModel.BuildEmpty();
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder
            .Logging
            .ClearProviders()
            .AddNLog()
            .AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });

        string _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? builder.Environment.EnvironmentName;
        string _modePrefix = builder.Configuration.ReadBaseOptions(args, _environmentName);
        builder.Services.AddOptions();

        //ITraceRabbitActionsService.TracesRequiredQueues = builder.Configuration.GetSection(nameof(ITraceRabbitActionsService.TracesRequiredQueues)).Get<string[]>();
        _confMQTT.Reload(builder.Configuration.GetSection(RealtimeMQTTClientConfigModel.Configuration).Get<RealtimeMQTTClientConfigModel>()!);
        logger.Warn($"mqtt config: {JsonConvert.SerializeObject(_confMQTT)}");
        builder.Services.AddSingleton(sp => _confMQTT);

        builder.WebHost.ConfigureKestrel((b, o) =>
        {
            // This will allow MQTT connections based on TCP port 1883.
            o.ListenAnyIP(_confMQTT.Port, l => l.UseMqtt());
            // This will allow MQTT connections based on HTTP WebSockets with URI "localhost:5000/mqtt"
            // See code below for URI configuration.
            //o.ListenAnyIP(3883); // Default HTTP pipeline
        });
        builder.AddServiceDefaults();

        builder.Services.AddMemoryCache();

        string connectionNlogsString = builder.Configuration.GetConnectionString($"NLogsConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'NLogsConnection{_modePrefix}' not found.");
        builder.Services.AddDbContextFactory<NLogsContext>(opt =>
            opt.UseNpgsql(connectionNlogsString));

        string connectionRealtime = builder.Configuration.GetConnectionString($"RealtimeConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'RealtimeConnection{_modePrefix}' not found.");
        builder.Services.AddDbContextFactory<RealtimeContext>(opt =>
            opt.UseNpgsql(connectionRealtime));

        builder.Services
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString($"RedisConnectionString{_modePrefix}");
            })
            .AddSingleton<IManualCustomCacheService, ManualCustomCacheService>()
            .AddSingleton<IIdentityTransmission, IdentityTransmissionRabbit>()
        ;

        builder.Services.AddSingleton(sp => _confMQTT);

        builder.Services
            .Configure<RabbitMQConfigModel>(builder.Configuration.GetSection(RabbitMQConfigModel.Configuration))
        ;

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSingleton<MqttHostedServer>();
        builder.Services.AddHostedMqttServer(
        optionsBuilder =>
        {
            optionsBuilder.WithDefaultEndpoint();
        });
        builder.Services.AddSingleton<MqttServer>(s => s.GetRequiredService<MqttHostedServer>());

        builder.Services.AddMqttConnectionHandler();
        builder.Services.AddConnections();

        builder.Services
           .AddScoped<IWebChatService, WebChatService>()
           ;

        builder.Services.AddTransient<MqttController>();

        string appName = typeof(Program).Assembly.GetName().Name ?? "AssemblyName";
        #region MQ Transmission (remote methods call)
        builder.Services.AddSingleton<RabbitMQ.Client.ConnectionFactory>(sp =>
        {
            IOptions<RabbitMQConfigModel> rabbitConf = sp.GetRequiredService<IOptions<RabbitMQConfigModel>>();

            return new()
            {
                ClientProvidedName = rabbitConf.Value.ClientProvidedName,
                HostName = rabbitConf.Value.HostName,
                UserName = rabbitConf.Value.UserName,
                Password = rabbitConf.Value.Password
            };
        });
        builder.Services.AddSingleton(sp =>
        {

            RabbitMQ.Client.ConnectionFactory factory = sp.GetRequiredService<RabbitMQ.Client.ConnectionFactory>();
            return factory.CreateConnectionAsync().Result;
        });

        builder.Services
            .AddSingleton<IMQStandardClientExtRPC>(x => new ClientMQTT(x.GetRequiredService<RealtimeMQTTClientConfigModel>(), x.GetRequiredService<ILogger<ClientMQTT>>(), appName))
            ;

        IMQStandardClientRPC rabbitImplement(IServiceProvider provider, object arg2)
        {
            return new RabbitClient(
                provider.GetRequiredService<IOptions<RabbitMQConfigModel>>(),
                provider.GetRequiredService<ILogger<RabbitClient>>(),
                provider.GetRequiredService<ITraceRabbitActionsServiceTransmission>(),
                _confMQTT,
                appName);
        }

        builder.Services
            .AddKeyedSingleton(nameof(RabbitClient), rabbitImplement)
            ;
        //
        builder.Services
            .AddSingleton<ITraceRabbitActionsServiceTransmission, TraceRabbitActionsTransmission>()
            .AddScoped<IEventsWebChatsNotifies, EventsWebChatsNotifiesTransmissionMQTT>()
            .AddScoped<IWebTransmission, WebTransmissionRabbit>()
            .AddScoped<IIdentityTransmission, IdentityTransmissionRabbit>()
            .AddScoped<ITelegramTransmission, TelegramTransmissionRabbit>()
            .AddScoped<IHelpdeskTransmission, HelpdeskTransmissionRabbit>()
            .AddScoped<IRubricsService, RubricsTransmissionRabbit>()
            .AddScoped<ICommerceTransmission, CommerceTransmissionRabbit>()
            .AddScoped<IKladrNavigationService, KladrNavigationServiceTransmissionRabbit>()
            .AddScoped<IStorageTransmission, StorageTransmissionRabbit>()
            .AddScoped<IParametersStorageTransmission, ParametersStorageTransmissionRabbit>()
            .AddScoped<IKladrService, KladrServiceTransmissionRabbit>()
            .AddScoped<IBankService, BankTransmissionRabbit>()
            .AddScoped<IMerchantService, MerchantTransmissionRabbit>()
            .AddScoped<IFirebaseServiceTransmission, FirebaseTransmissionRabbit>()
            ;
        #endregion

        builder.Services.RealtimeRegisterListenersRabbitMQ();

        // Custom metrics for the application
        Meter greeterMeter = new($"OTel.{appName}", "1.0.0");
        OpenTelemetryBuilder otel = builder.Services.AddOpenTelemetry();

        // Add Metrics for ASP.NET Core and our custom metrics and export via OTLP
        otel.WithMetrics(metrics =>
        {
            // Metrics provider from OpenTelemetry
            metrics.AddAspNetCoreInstrumentation();
            //Our custom metrics
            metrics.AddMeter(greeterMeter.Name);
            // Metrics provides by ASP.NET Core in .NET 8
            metrics.AddMeter("Microsoft.AspNetCore.Hosting");
            metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
        });

        // Add Tracing for ASP.NET Core and our custom ActivitySource and export via OTLP
        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
            tracing.AddSource($"OTel.{appName}");
        });

        WebApplication app = builder.Build();
        app.UseRouting();

#pragma warning disable ASP0014 // Suggest using top level route registrations
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapConnectionHandler<MqttConnectionHandler>(
                    "/mqtt",
                    httpConnectionDispatcherOptions =>
                    {
                        httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector = protocolList => protocolList.FirstOrDefault() ?? string.Empty;
                    });
            });
#pragma warning restore ASP0014 // Suggest using top level route registrations
        app.UseMqttServer(
            server =>
            {
                server.ValidatingConnectionAsync += async (e) =>
                {
                    using IServiceScope ss = app.Services.CreateScope();
                    MqttController mq_cli = ss.ServiceProvider.GetRequiredService<MqttController>();
                    await mq_cli.ValidateConnection(e);
                };
                server.ClientConnectedAsync += async (e) =>
                {
                    using IServiceScope ss = app.Services.CreateScope();
                    MqttController mq_cli = ss.ServiceProvider.GetRequiredService<MqttController>();
                    await mq_cli.ClientConnected(e);
                };
                server.ClientDisconnectedAsync += async (e) =>
                {
                    using IServiceScope ss = app.Services.CreateScope();
                    MqttController mq_cli = ss.ServiceProvider.GetRequiredService<MqttController>();
                    await mq_cli.ClientDisconnected(e);
                };
                server.ClientSubscribedTopicAsync += async (e) =>
                {
                    using IServiceScope ss = app.Services.CreateScope();
                    MqttController mq_cli = ss.ServiceProvider.GetRequiredService<MqttController>();
                    await mq_cli.ClientSubscribedTopic(e);
                };
                server.PreparingSessionAsync += async (e) =>
                {
                    using var ss = app.Services.CreateScope();
                    MqttController mq_cli = ss.ServiceProvider.GetRequiredService<MqttController>();
                    await mq_cli.PreparingSession(e);
                };
            });

        app.Run();
    }
}