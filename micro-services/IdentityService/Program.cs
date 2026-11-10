////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;
using NLog.Extensions.Logging;
using OpenTelemetry.Metrics;
using System.Globalization;
using OpenTelemetry.Trace;
using Newtonsoft.Json;
using OpenTelemetry;
using RemoteCallLib;
using ServicesLib;
using IdentityLib;
using System.Text;
using ServerLib;
using SharedLib;
using NLog;

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
        RealtimeMQTTClientConfigModel _confMQTT = RealtimeMQTTClientConfigModel.BuildEmpty();
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
        string _modePrefix = builder.Configuration.ReadBaseOptions(args, _environmentName);
        builder.Services.AddOptions();

        //ITraceRabbitActionsService.TracesRequiredQueues = builder.Configuration.GetSection(nameof(ITraceRabbitActionsService.TracesRequiredQueues)).Get<string[]>();
        _confMQTT.Reload(builder.Configuration.GetSection(RealtimeMQTTClientConfigModel.Configuration).Get<RealtimeMQTTClientConfigModel>()!);
        logger.Warn($"mqtt config: {JsonConvert.SerializeObject(_confMQTT)}");
        builder.Services.AddSingleton(sp => _confMQTT);

        builder.Services
            .Configure<RabbitMQConfigModel>(builder.Configuration.GetSection(RabbitMQConfigModel.Configuration))
            .Configure<SmtpConfigModel>(builder.Configuration.GetSection(SmtpConfigModel.Configuration))
            ;

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
        builder.Services
            .AddScoped<IIdentityTools, IdentityTools>()
            .AddScoped<IIndexingServive, IndexingTransmissionRabbit>()
            .AddScoped<IHistoryIndexing, HistoryTransmissionRabbit>()
            ;

        string appName = typeof(Program).Assembly.GetName().Name ?? "AssemblyName";
        builder.Services
            .AddSingleton<IMQStandardClientExtRPC>(x => new ClientMQTT(x.GetRequiredService<RealtimeMQTTClientConfigModel>(), x.GetRequiredService<ILogger<ClientMQTT>>(), appName))
            ;

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
            .AddSingleton<ITraceRabbitActionsServiceTransmission, TraceRabbitActionsTransmission>()
            .AddScoped<ITelegramTransmission, TelegramTransmissionRabbit>()
            .AddScoped<IHelpdeskTransmission, HelpdeskTransmissionRabbit>()
            .AddScoped<IWebTransmission, WebTransmissionRabbit>()
            .AddScoped<IStorageTransmission, StorageTransmissionRabbit>()
            .AddScoped<IParametersStorageTransmission, ParametersStorageTransmissionRabbit>()
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