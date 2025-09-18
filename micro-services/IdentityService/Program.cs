////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using RemoteCallLib;
using IdentityLib;
using ServerLib;
using SharedLib;
using NLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using OpenTelemetry;
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Options;
using System.Text;

namespace IdentityService;

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
        Logger logger = LogManager.GetCurrentClassLogger();

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

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
        string _modePrefix = Environment.GetEnvironmentVariable(nameof(GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix)) ?? "";
        if (!string.IsNullOrWhiteSpace(_modePrefix) && !GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix.EndsWith(_modePrefix))
            GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix += _modePrefix.Trim();
        string curr_dir = Directory.GetCurrentDirectory();


        builder.Configuration.SetBasePath(curr_dir);
        string path_load = Path.Combine(curr_dir, "appsettings.json");
        if (Path.Exists(path_load))
            builder.Configuration.AddJsonFile(path_load, optional: true, reloadOnChange: false);

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
            .Configure<SmtpConfigModel>(builder.Configuration.GetSection(SmtpConfigModel.Configuration))
            ;

        builder.Services.AddOptions();
        builder.Services.AddMemoryCache();
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString($"RedisConnectionString{_modePrefix}");
        })
        .AddSingleton<IManualCustomCacheService, ManualCustomCacheService>();


        string connectionIdentityString = builder.Configuration.GetConnectionString($"IdentityConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'IdentityConnection{_modePrefix}' not found.");
        builder.Services.AddDbContextFactory<IdentityAppDbContext>(opt =>
            opt.UseNpgsql(connectionIdentityString));

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;

        })//.AddUserManager<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddRoleManager<RoleManager<ApplicationRole>>()
            .AddEntityFrameworkStores<IdentityAppDbContext>()
            .AddDefaultTokenProviders()
            ;

        builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AdditionalUserClaimsPrincipalFactory>();


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        //    builder.Services.AddAuthorization(options =>
        //options.AddPolicy("TwoFactorEnabled", x => x.RequireClaim("amr", "mfa")));

        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        builder.Services.AddLocalization(lo => lo.ResourcesPath = "Resources");
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            CultureInfo[] supportedCultures = [new CultureInfo("en-US"), GlobalStaticConstants.RU];
            options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            IRequestCultureProvider? defaultCookieRequestProvider =
                options.RequestCultureProviders.FirstOrDefault(rcp =>
                    rcp.GetType() == typeof(CookieRequestCultureProvider));
            if (defaultCookieRequestProvider != null)
                options.RequestCultureProviders.Remove(defaultCookieRequestProvider);

            options.RequestCultureProviders.Insert(0,
                new CookieRequestCultureProvider()
                {
                    CookieName = ".AspNetCore.Culture",
                    Options = options
                });
        });

        //Singleton
        builder.Services
            .AddSingleton<IMailProviderService, MailProviderService>()
            .AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSender>()
            ;

        // Scoped
        builder.Services.AddScoped<IIdentityTools, IdentityTools>();

        string appName = typeof(Program).Assembly.GetName().Name ?? "AssemblyName";
        #region MQ Transmission (remote methods call)
        builder.Services.AddSingleton<IRabbitClient>(x => new RabbitClient(x.GetRequiredService<IOptions<RabbitMQConfigModel>>(), x.GetRequiredService<ILogger<RabbitClient>>(), appName));

        builder.Services
            .AddScoped<ITelegramTransmission, TelegramTransmission>()
            .AddScoped<IHelpDeskTransmission, HelpDeskTransmission>()
            .AddScoped<IWebTransmission, WebTransmission>()
            .AddScoped<IStorageTransmission, StorageTransmission>()
            .AddScoped<IParametersStorageTransmission, ParametersStorageTransmission>()
            ;

        builder.Services.IdentityRegisterMqListeners();
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

        builder.Services.AddOptions();

        IHost host = builder.Build();
        host.Run();
    }
}