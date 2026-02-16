////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DbcLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using RemoteCallLib;
using SharedLib;
using System.Diagnostics.Metrics;
using System.Text;

namespace RealtimeService;

public class Program
{
    static Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    static MQTTClientConfigModel _conf = MQTTClientConfigModel.BuildEmpty();
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
        logger.Warn($"init main: {_environmentName}");

        string _modePrefix = Environment.GetEnvironmentVariable(nameof(GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix)) ?? "";
        if (!string.IsNullOrWhiteSpace(_modePrefix) && !GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix.EndsWith(_modePrefix))
        {
            GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix += _modePrefix.Trim();
            GlobalStaticConstantsTransmission.TransmissionQueueNamePrefixMQTT += _modePrefix.Trim();
        }

        logger.Warn($"Префикс рабочего контура/контекста: {(string.IsNullOrWhiteSpace(_modePrefix) ? "НЕ ИСПОЛЬЗУЕТСЯ" : $"`{_modePrefix}`")}");

        string curr_dir = Directory.GetCurrentDirectory();
        builder.Configuration.SetBasePath(curr_dir);
        string path_load = Path.Combine(curr_dir, "appsettings.json");
        if (Path.Exists(path_load))
        {
            logger.Warn($"config load: {path_load}\n{File.ReadAllText(path_load)}");
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        }
        else
            logger.Warn($"отсутствует: {path_load}");

        path_load = Path.Combine(curr_dir, $"appsettings.{_environmentName}.json");
        if (Path.Exists(path_load))
        {
            logger.Warn($"config load: {path_load}\n{File.ReadAllText(path_load)}");
            builder.Configuration.AddJsonFile(path_load, optional: true, reloadOnChange: false);
        }
        else
            logger.Warn($"отсутствует: {path_load}");

        // Secrets
        void ReadSecrets(string dirName)
        {
            string secretPath = Path.Combine("..", dirName);
            DirectoryInfo di = new(secretPath);
            for (int i = 0; i < 5 && !di.Exists; i++)
            {
                logger.Warn($"файл секретов не найден (продолжение следует...): {di.FullName}");
                secretPath = Path.Combine("..", secretPath);
                di = new(secretPath);
            }

            if (Directory.Exists(di.FullName))
            {
                foreach (string secret in Directory.GetFiles(secretPath, $"*.json"))
                {
                    path_load = Path.GetFullPath(secret);
                    logger.Warn($"!secret load: {path_load}");
                    builder.Configuration.AddJsonFile(path_load, optional: true, reloadOnChange: false);
                }
            }
            else
                logger.Warn($"Секреты `{dirName}` не найдены (совсем)");
        }
        ReadSecrets("secrets");
        if (!string.IsNullOrWhiteSpace(_modePrefix))
            ReadSecrets($"secrets{_modePrefix}");

        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddCommandLine(args);

        builder.Services
            .Configure<RabbitMQConfigModel>(builder.Configuration.GetSection(RabbitMQConfigModel.Configuration))
        ;
        _conf.Reload(builder.Configuration.GetSection("RealtimeConfig").Get<MQTTClientConfigModel>()!);

        builder.WebHost.ConfigureKestrel((b, o) =>
        {
            // This will allow MQTT connections based on TCP port 1883.
            o.ListenAnyIP(1883, l => l.UseMqtt());

            // This will allow MQTT connections based on HTTP WebSockets with URI "localhost:5000/mqtt"
            // See code below for URI configuration.
            o.ListenAnyIP(3883); // Default HTTP pipeline
        });
        builder.AddServiceDefaults();

        builder.Services.AddOptions();
        builder.Services.AddMemoryCache();

        string connectionNlogsString = builder.Configuration.GetConnectionString($"NLogsConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'NLogsConnection{_modePrefix}' not found.");
        builder.Services.AddDbContextFactory<NLogsContext>(opt =>
            opt.UseNpgsql(connectionNlogsString));

        string connectionRealtime = builder.Configuration.GetConnectionString($"RealtimeConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'RealtimeConnection{_modePrefix}' not found.");
        builder.Services.AddDbContextFactory<RealtimeContext>(opt =>
            opt.UseNpgsql(connectionRealtime));

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString($"RedisConnectionString{_modePrefix}");
        })
        .AddSingleton<IManualCustomCacheService, ManualCustomCacheService>();

        builder.Services.AddSingleton(sp => _conf);

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

        builder.Services
            .AddSingleton<IMQTTClient>(x => new MQttClient(x.GetRequiredService<MQTTClientConfigModel>(), x.GetRequiredService<ILogger<MQttClient>>(), appName))
            ;

        builder.Services.AddSingleton<IRabbitClient>(x => new RabbitClient(x.GetRequiredService<IOptions<RabbitMQConfigModel>>(), x.GetRequiredService<ILogger<RabbitClient>>(), appName));
        //
        builder.Services
            .AddScoped<IEventsWebChatsNotifies, EventsWebChatsNotifiesTransmissionMQTT>()
            .AddScoped<IWebTransmission, WebTransmission>()
            .AddScoped<IIdentityTransmission, IdentityTransmission>()
            .AddScoped<ITelegramTransmission, TelegramTransmission>()
            .AddScoped<IHelpDeskTransmission, HelpDeskTransmission>()
            .AddScoped<IRubricsTransmission, RubricsTransmission>()
            .AddScoped<ICommerceTransmission, CommerceTransmission>()
            .AddScoped<IKladrNavigationService, KladrNavigationServiceTransmission>()
            .AddScoped<IStorageTransmission, StorageTransmission>()
            .AddScoped<IParametersStorageTransmission, ParametersStorageTransmission>()
            .AddScoped<IKladrService, KladrServiceTransmission>()
            .AddScoped<IBankService, BankTransmission>()
            .AddScoped<IMerchantService, MerchantTransmission>()
            ;
        #endregion
        builder.Services.RealtimeRegisterMqListeners();
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
                    httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                        protocolList => protocolList.FirstOrDefault() ?? string.Empty);
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
/// <summary>
/// EventNotifyExtensions
/// </summary>
public static class EventNotifyExtensions
{
    /// <inheritdoc/>
    public static IServiceCollection RegisterEventNotify<T>(this IServiceCollection services)
    {
        services.AddTransient<IEventNotifyReceive<T>, EventNotifyReceive<T>>();
        return services;
    }
}