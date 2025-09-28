////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;
using NLog.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using FluentScheduler;
using OpenTelemetry;
using RemoteCallLib;
using BankService;
using System.Text;
using SharedLib;
using NLog.Web;
using DbcLib;
using NLog;
using TinkoffPaymentClientApi.ResponseEntity;
using Newtonsoft.Json;

Console.OutputEncoding = Encoding.UTF8;
// Early init of NLog to allow startup and exception logging, before host is built
Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
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
logger.Warn($"init main: {_environmentName}");

string _modePrefix = Environment.GetEnvironmentVariable(nameof(GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix)) ?? "";
if (!string.IsNullOrWhiteSpace(_modePrefix) && !GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix.EndsWith(_modePrefix))
    GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix += _modePrefix.Trim();

logger.Warn($"Префикс рабочего контура/контекста: {(string.IsNullOrWhiteSpace(_modePrefix) ? "НЕ ИСПОЛЬЗУЕТСЯ" : $"`{_modePrefix}`")}");

string curr_dir = Directory.GetCurrentDirectory();
builder.Configuration.SetBasePath(curr_dir);

builder.Configuration.SetBasePath(curr_dir);
string path_load = Path.Combine(curr_dir, "appsettings.json");
if (Path.Exists(path_load))
    builder.Configuration.AddJsonFile(path_load, optional: true, reloadOnChange: false);
else
    logger.Warn($"отсутствует: {path_load}");

path_load = Path.Combine(curr_dir, $"appsettings.{_environmentName}.json");
if (Path.Exists(path_load))
    builder.Configuration.AddJsonFile(path_load, optional: true, reloadOnChange: false);
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

    if (Directory.Exists(secretPath))
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
.Configure<TBankSettings>(builder.Configuration.GetSection(nameof(TBankSettings)))
;

builder.Services
    .AddScoped<IBankService, BankImplementService>()
    .AddScoped<IMerchantService, MerchantImplementService>()
    ;

builder.Services.AddSingleton<WebConfigModel>();
builder.Services.AddOptions();
string connectionString = builder.Configuration.GetConnectionString($"BankConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'BankConnection{_modePrefix}' not found.");
builder.Services.AddDbContextFactory<BankContext>(opt =>
{
    opt.UseNpgsql(connectionString);

#if DEBUG
    opt.EnableSensitiveDataLogging(true);
    opt.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
#endif
});

builder.Services.AddMemoryCache();

string appName = typeof(Program).Assembly.GetName().Name ?? "AssemblyName";
#region MQ Transmission (remote methods call)
builder.Services.AddSingleton<IRabbitClient>(x => new RabbitClient(x.GetRequiredService<IOptions<RabbitMQConfigModel>>(), x.GetRequiredService<ILogger<RabbitClient>>(), appName));
//
builder.Services.AddScoped<IWebTransmission, WebTransmission>()
    .AddScoped<ITelegramTransmission, TelegramTransmission>()
    .AddScoped<IHelpDeskTransmission, HelpDeskTransmission>()
    .AddScoped<IStorageTransmission, StorageTransmission>()
    .AddScoped<IIdentityTransmission, IdentityTransmission>()
    .AddScoped<IParametersStorageTransmission, ParametersStorageTransmission>()
    .AddScoped<ICommerceTransmission, CommerceTransmission>()
    ;
//
builder.Services.BankRegisterMqListeners();
#endregion

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

JobManager.Initialize();
JobManager.JobException += JobManager_JobException;
JobManager.JobStart += JobManager_JobStart;

void JobManager_JobStart(JobStartInfo info)
{
    logger.Info($"FluentJob `{info.Name}`: started");
}

JobManager.JobEnd += JobManager_JobEnd;

void JobManager_JobEnd(JobEndInfo info)
{
    logger.Info($"FluentJob `{info.Name}`: ended ({info.Duration})");
}

void JobManager_JobException(JobExceptionInfo info)
{
    logger.Error("An error just happened with a scheduled job: " + info.Exception);
}

JobManager.AddJob(
    () => Console.WriteLine("5 minutes just passed."),
    s => s.WithName("simpe tet job").ToRunEvery(5).Minutes()
);

host.Run();