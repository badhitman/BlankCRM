using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlankBlazorApp.Components;
using System.Globalization;
using MudBlazor.Services;
using RemoteCallLib;
using IdentityLib;
using ServerLib;
using SharedLib;
using BlazorLib;
using NLog.Web;
using DbcLib;
using NLog;
using BlazorWebLib;
using BlankBlazorApp;
using OpenTelemetry.Metrics;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry;
using Microsoft.Extensions.Options;
using System.Text;
#if !DEBUG
using System.Reflection;
#endif

Console.OutputEncoding = Encoding.UTF8;
// Early init of NLog to allow startup and exception logging, before host is built
Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders()
    .AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});
builder.Host.UseNLog();
string _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? builder.Environment.EnvironmentName;
logger.Warn($"init main: {_environmentName}");

string _modePrefix = Environment.GetEnvironmentVariable(nameof(GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix)) ?? "";
if (!string.IsNullOrWhiteSpace(_modePrefix) && !GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix.EndsWith(_modePrefix))
    GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix += _modePrefix.Trim();

string curr_dir = Directory.GetCurrentDirectory();
builder.Configuration.SetBasePath(curr_dir);
string path_load = Path.Combine(curr_dir, "appsettings.json");
if (Path.Exists(path_load))
{
    logger.Warn($"config load: {path_load}\n{File.ReadAllText(path_load)}");
    builder.Configuration.AddJsonFile(path_load, optional: true, reloadOnChange: false);
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

path_load = Path.Combine(curr_dir, $"bottom-menu.json");
if (Path.Exists(path_load))
{
    logger.Warn($"config load: {path_load}\n{File.ReadAllText(path_load)}");
    builder.Configuration.AddJsonFile(path_load, optional: true, reloadOnChange: false);
}
else
    logger.Warn($"отсутствует: {path_load}");

path_load = Path.Combine(curr_dir, $"bottom-menu.{_environmentName}.json");
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

builder.Services.AddIdleCircuitHandler(options =>
    options.IdleTimeout = TimeSpan.FromMinutes(5));

builder.Services.AddOptions();
builder.Services
    .Configure<UserManageConfigModel>(builder.Configuration.GetSection("UserManage"))
    .Configure<ServerConfigModel>(builder.Configuration.GetSection("ServerConfig"))
    .Configure<RabbitMQConfigModel>(builder.Configuration.GetSection(RabbitMQConfigModel.Configuration))
    .Configure<TelegramBotConfigModel>(builder.Configuration.GetSection(WebConfigModel.Configuration))
    ;

NavMainMenuModel? mainNavMenu = builder.Configuration.GetSection("NavMenuConfig").Get<NavMainMenuModel>();
mainNavMenu ??= new NavMainMenuModel() { NavMenuItems = [new NavItemModel() { HrefNav = "", Title = "Home", IsNavLinkMatchAll = true }] };
builder.Services.AddCascadingValue(sp => mainNavMenu);

string connectionIdentityString = builder.Configuration.GetConnectionString($"IdentityConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'IdentityConnection{_modePrefix}' not found.");
builder.Services.AddDbContextFactory<IdentityAppDbContext>(opt =>
    opt.UseNpgsql(connectionIdentityString));

string connectionMainString = builder.Configuration.GetConnectionString($"MainConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'MainConnection{_modePrefix}' not found.");
builder.Services.AddDbContextFactory<MainAppContext>(opt =>
    opt.UseNpgsql(connectionMainString));

builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

builder.Services.AddMemoryCache();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<ApplicationRole>()
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddEntityFrameworkStores<IdentityAppDbContext>()
    .AddSignInManager()
    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
    .AddDefaultTokenProviders()
    ;

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

builder.Services.AddHttpContextAccessor();

//Singleton
builder.Services
    .AddSingleton<IMailProviderService, MailProviderService>()
    .AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSender>()
    ;

// Scoped
builder.Services.AddScoped<IUsersAuthenticateService, UsersAuthenticateService>()
    .AddScoped<IUsersProfilesService, UsersProfilesService>()
    .AddScoped<ILogsService, LogsService>()
    ;

#region MQ Transmission (remote methods call)
string appName = typeof(Program).Assembly.GetName().Name ?? "AssemblyName";
builder.Services.AddSingleton<IRabbitClient>(x =>
    new RabbitClient(x.GetRequiredService<IOptions<RabbitMQConfigModel>>(),
                x.GetRequiredService<ILogger<RabbitClient>>(),
                appName));
//
builder.Services
    .AddScoped<ICommerceTransmission, CommerceTransmission>()
    .AddScoped<ITelegramTransmission, TelegramTransmission>()
    .AddScoped<IHelpDeskTransmission, HelpDeskTransmission>()
    .AddScoped<IRubricsTransmission, RubricsTransmission>()
    .AddScoped<IStorageTransmission, StorageTransmission>()
    .AddScoped<IKladrNavigationService, KladrNavigationServiceTransmission>()
    .AddScoped<IConstructorTransmission, ConstructorTransmission>()
    .AddScoped<IIdentityTransmission, IdentityTransmission>()
    .AddScoped<IWebTransmission, WebTransmission>()

    .AddScoped<IDaichiBusinessApiTransmission, DaichiBusinessTransmission>()
    .AddScoped<IRusklimatComApiTransmission, RusklimatComTransmission>()
    .AddScoped<IFeedsHaierProffRuService, HaierProffRuTransmission>()
    .AddScoped<IBreezRuApiTransmission, BreezRuTransmission>()
    ;

builder.Services.WebAppRegisterMqListeners();
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
    metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
});

// Add Tracing for ASP.NET Core and our custom ActivitySource and export via OTLP
otel.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddSource($"OTel.{appName}");
    tracing.AddHttpClientInstrumentation();
});

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Append("Content-Security-Policy", "{POLICY STRING}");
//    await next();
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Map("/cloud-fs/read", ma => ma.UseMiddleware<ReadCloudFileMiddleware>());

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlankBlazorApp.Client._Imports).Assembly, typeof(BlazorWebLib._Imports).Assembly, typeof(BlazorLib._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

#if !DEBUG
Task? init_email_notify = null;
try
{
    init_email_notify = app.Services
        .GetRequiredService<IMailProviderService>()
        .SendTechnicalEmailNotificationAsync($"init main{(string.IsNullOrWhiteSpace(_modePrefix) ? "" : $" (prefix mode: {_modePrefix})")}: {Assembly.GetEntryAssembly()?.FullName}");

    if (init_email_notify is not null)
        await init_email_notify;
    else
        logger.Error($"init_email_notify is null. error {{807BC02B-E15E-4840-A564-FBC50CFBFFB8}}");
}
catch (Exception ex)
{
    logger.Error(ex);
}
#endif

app.Run();