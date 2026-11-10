using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;
using NLog.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Newtonsoft.Json;
using RemoteCallLib;
using OpenTelemetry;
using System.Text;
using ServicesLib;
using SharedLib;
using DbcLib;
using NLog;

namespace KladrService;

public class Program
{
    public static void Main(string[] args)
    {
        RealtimeMQTTClientConfigModel _confMQTT = RealtimeMQTTClientConfigModel.BuildEmpty();
        Console.OutputEncoding = Encoding.UTF8;
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        Logger logger = LogManager.GetCurrentClassLogger();
        builder.AddServiceDefaults();

        // NLog: Setup NLog for Dependency injection
        builder.Logging
            .ClearProviders()
            .AddNLog()
            .AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true; logging.IncludeScopes = true;
            });

        string _environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Other";
        string _modePrefix = builder.Configuration.ReadBaseOptions(args, _environmentName);
        builder.Services.AddOptions();

        //ITraceRabbitActionsService.TracesRequiredQueues = builder.Configuration.GetSection(nameof(ITraceRabbitActionsService.TracesRequiredQueues)).Get<string[]>();
        _confMQTT.Reload(builder.Configuration.GetSection(RealtimeMQTTClientConfigModel.Configuration).Get<RealtimeMQTTClientConfigModel>()!);
        logger.Warn($"mqtt config: {JsonConvert.SerializeObject(_confMQTT)}");
        builder.Services.AddSingleton(sp => _confMQTT);

        builder.Services
            .Configure<RabbitMQConfigModel>(builder.Configuration.GetSection(RabbitMQConfigModel.Configuration))
            ;

        builder.Services.AddMemoryCache();

        RabbitMQConfigModel _mqConf = builder.Configuration.GetSection(RabbitMQConfigModel.Configuration).Get<RabbitMQConfigModel>() ?? throw new Exception("RabbitMQ not config");

        string connectionStorage = builder.Configuration.GetConnectionString($"KladrConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'KladrConnection{_modePrefix}' not found.");
        builder.Services.AddDbContextFactory<KladrContext>(opt =>
            opt.UseNpgsql(connectionStorage));

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

        IMQStandardClientRPC rabbitImplement(IServiceProvider provider, object? arg2)
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
        builder.Services
            .AddSingleton<IMQStandardClientExtRPC>(x => new ClientMQTT(x.GetRequiredService<RealtimeMQTTClientConfigModel>(), x.GetRequiredService<ILogger<ClientMQTT>>(), appName))
            ;

        builder.Services.KladrRegisterMqListeners();
        builder.Services
            .AddSingleton<ITraceRabbitActionsServiceTransmission, TraceRabbitActionsTransmission>()
            ;
        #endregion
        builder.Services
            .AddScoped<IKladrService, KladrServiceImpl>()
            .AddScoped<IKladrNavigationService, KladrNavigationServiceImpl>()
            ;

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
        });

        builder.Services.AddHttpClient(HttpClientsNamesEnum.RabbitMqManagement.ToString(), cc =>
        {
            cc.BaseAddress = new Uri($"http://{_mqConf.HostName}:{_mqConf.PortManagementPlugin}/");
            string authenticationString = $"{_mqConf.UserName}:{_mqConf.Password}";
            string base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));
            cc.DefaultRequestHeaders.Add("Authorization", $"Basic {base64EncodedAuthenticationString}");
        });

        // Add Tracing for ASP.NET Core and our custom ActivitySource and export via OTLP
        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
            tracing.AddSource($"OTel.{appName}");
        });

        IHost host = builder.Build();
        host.Run();
    }
}