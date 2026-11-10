////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;
using NLog.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using ConstructorService;
using Newtonsoft.Json;
using RemoteCallLib;
using OpenTelemetry;
using ServicesLib;
using System.Text;
using SharedLib;
using DbcLib;
using NLog;

RealtimeMQTTClientConfigModel _confMQTT = RealtimeMQTTClientConfigModel.BuildEmpty();
Console.OutputEncoding = Encoding.UTF8;
// Early init of NLog to allow startup and exception logging, before host is built
Logger logger = LogManager.Setup().LoadConfigurationFromFile().GetCurrentClassLogger();
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder
    .Logging
    .ClearProviders()
    .AddNLog()
    .AddOpenTelemetry(logging =>
    {
        logging.IncludeFormattedMessage = true;
        logging.IncludeScopes = true;
    });

string _environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? builder.Environment.EnvironmentName;
string _modePrefix = builder.Configuration.ReadBaseOptions(args, _environmentName);
builder.Services.AddOptions();

//ITraceRabbitActionsService.TracesRequiredQueues = builder.Configuration.GetSection(nameof(ITraceRabbitActionsService.TracesRequiredQueues)).Get<string[]>();
_confMQTT.Reload(builder.Configuration.GetSection(RealtimeMQTTClientConfigModel.Configuration).Get<RealtimeMQTTClientConfigModel>()!);
logger.Warn($"mqtt config: {JsonConvert.SerializeObject(_confMQTT)}");
builder.Services.AddSingleton(sp => _confMQTT);

builder.Services
   .Configure<RabbitMQConfigModel>(builder.Configuration.GetSection(RabbitMQConfigModel.Configuration))
   .Configure<ConstructorConfigModel>(builder.Configuration.GetSection("ConstructorConfig"));

string connectionString = builder.Configuration.GetConnectionString($"ConstructorConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'ConstructorConnection{_modePrefix}' not found.");
builder.Services.AddDbContextFactory<ConstructorContext>(opt =>
{
    opt.UseNpgsql(connectionString);

#if DEBUG
    opt.EnableSensitiveDataLogging(true);
    opt.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
#endif
});

builder.Services.AddMemoryCache();
builder.Services
    .AddScoped<IConstructorService, FormsConstructorService>()
    ;

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
//
builder.Services
    .AddSingleton<ITraceRabbitActionsServiceTransmission, TraceRabbitActionsTransmission>()
    .AddScoped<IIndexingServive, IndexingTransmissionRabbit>()
    .AddScoped<IHistoryIndexing, HistoryTransmissionRabbit>()
    .AddScoped<IWebTransmission, WebTransmissionRabbit>()
    .AddScoped<ITelegramTransmission, TelegramTransmissionRabbit>()
    .AddScoped<IHelpdeskTransmission, HelpdeskTransmissionRabbit>()
    .AddScoped<IKladrNavigationService, KladrNavigationServiceTransmissionRabbit>()
    .AddScoped<IStorageTransmission, StorageTransmissionRabbit>()
    .AddScoped<IParametersStorageTransmission, ParametersStorageTransmissionRabbit>()
    .AddScoped<IIdentityTransmission, IdentityTransmissionRabbit>()
    ;
//
builder.Services.ConstructorRegisterMqListeners();
#endregion

builder.Services
    .AddSingleton<IMQStandardClientExtRPC>(x => new ClientMQTT(x.GetRequiredService<RealtimeMQTTClientConfigModel>(), x.GetRequiredService<ILogger<ClientMQTT>>(), appName))
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

// Add Tracing for ASP.NET Core and our custom ActivitySource and export via OTLP
otel.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddHttpClientInstrumentation();
    tracing.AddSource($"OTel.{appName}");
});
IHost host = builder.Build();

using (IServiceScope ss = host.Services.CreateScope())
{
    IOptions<ConstructorConfigModel> wc_main = ss.ServiceProvider.GetRequiredService<IOptions<ConstructorConfigModel>>();
    IWebTransmission webRemoteCall = ss.ServiceProvider.GetRequiredService<IWebTransmission>();
    wc_main.Value.WebConfig = await webRemoteCall.GetWebConfigAsync();
}

host.Run();