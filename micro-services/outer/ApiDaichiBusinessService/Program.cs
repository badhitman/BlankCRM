////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;
using NLog.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using RemoteCallLib;
using OpenTelemetry;
using SharedLib;
using DbcLib;
using NLog;
using System.Text;

namespace ApiDaichiBusinessService;

/// <summary>
/// Program
/// </summary>
public class Program
{
    /// <summary>
    /// Main
    /// </summary>
    public static void Main(string[] args)
    {
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
        logger.Warn($"init main: {_environmentName}");
        string _modePrefix = Environment.GetEnvironmentVariable(nameof(GlobalStaticConstants.TransmissionQueueNamePrefix)) ?? "";
        if (!string.IsNullOrWhiteSpace(_modePrefix) && !GlobalStaticConstants.TransmissionQueueNamePrefix.EndsWith(_modePrefix))
            GlobalStaticConstants.TransmissionQueueNamePrefix += _modePrefix.Trim();

        string curr_dir = Directory.GetCurrentDirectory();

        builder.Configuration.SetBasePath(curr_dir);
        string path_load = Path.Combine(curr_dir, "appsettings.json");
        if (Path.Exists(path_load))
            builder.Configuration.AddJsonFile(path_load, optional: true, reloadOnChange: true);

        path_load = Path.Combine(curr_dir, $"appsettings.{_environmentName}.json");
        if (Path.Exists(path_load))
            builder.Configuration.AddJsonFile(path_load, optional: true, reloadOnChange: true);
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
                    builder.Configuration.AddJsonFile(path_load, optional: true, reloadOnChange: true);
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
            .Configure<TokenVersionModel>(builder.Configuration.GetSection("DaichiApiConfig"))
            ;

        builder.Services.AddMemoryCache();
        builder.Services.AddOptions();

        TokenVersionModel daichiCredentials = builder.Configuration.GetSection("DaichiApiConfig").Get<TokenVersionModel>() ?? throw new Exception("DaichiApi not config");
        RabbitMQConfigModel _mqConf = builder.Configuration.GetSection("RabbitMQConfig").Get<RabbitMQConfigModel>() ?? throw new Exception("RabbitMQ not config");

        string connectionStorage = builder.Configuration.GetConnectionString($"ApiDaichiBusinessConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'ApiDaichiBusinessConnection{_modePrefix}' not found.");
        builder.Services.AddDbContextFactory<ApiDaichiBusinessContext>(opt =>
            opt.UseNpgsql(connectionStorage));
        
        string appName = typeof(Program).Assembly.GetName().Name ?? "AssemblyName";
        #region MQ Transmission (remote methods call)
        builder.Services
            .AddSingleton<IRabbitClient>(x => new RabbitClient(x.GetRequiredService<IOptions<RabbitMQConfigModel>>(), x.GetRequiredService<ILogger<RabbitClient>>(), appName));

        builder.Services.ApiDaichiBusinessRegisterMqListeners();
        #endregion
        builder.Services
            .AddScoped<IDaichiBusinessApiTransmission, DaichiBusinessTransmission>()
            .AddScoped<IDaichiBusinessApiService, DaichiBusinessApiService>()
            ;

        builder.Services.AddHttpClient(HttpClientsNamesEnum.RabbitMqManagement.ToString(), cc =>
        {
            cc.BaseAddress = new Uri($"http://{_mqConf.HostName}:{_mqConf.PortManagementPlugin}/");
            string authenticationString = $"{_mqConf.UserName}:{_mqConf.Password}";
            string base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));
            cc.DefaultRequestHeaders.Add("Authorization", $"Basic {base64EncodedAuthenticationString}");
        });

        builder.Services.AddHttpClient(HttpClientsNamesOuterEnum.ApiDaichiBusiness.ToString(), cc => { cc.BaseAddress = new Uri($"https://api.daichi.ru/b2b/{daichiCredentials.Version}"); });

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
        host.Run();
    }
}