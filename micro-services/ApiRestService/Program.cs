////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using System.Diagnostics.Metrics;
using NLog.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Reflection;
using Microsoft.OpenApi;
using Newtonsoft.Json;
using ApiRestService;
using OpenTelemetry;
using RemoteCallLib;
using ServicesLib;
using System.Text;
using SharedLib;
using NLog.Web;
using DbcLib;
using NLog;

RealtimeMQTTClientConfigModel _confMQTT = RealtimeMQTTClientConfigModel.BuildEmpty();
Console.OutputEncoding = Encoding.UTF8;
Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

string _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? builder.Environment.EnvironmentName;
logger.Warn($"init main: {_environmentName}");

string _modePrefix = builder.Configuration.ReadBaseOptions(args, _environmentName);
builder.Services.AddOptions();

//ITraceRabbitActionsService.TracesRequiredQueues = builder.Configuration.GetSection(nameof(ITraceRabbitActionsService.TracesRequiredQueues)).Get<string[]>();
_confMQTT.Reload(builder.Configuration.GetSection(RealtimeMQTTClientConfigModel.Configuration).Get<RealtimeMQTTClientConfigModel>()!);
logger.Warn($"mqtt config: {JsonConvert.SerializeObject(_confMQTT)}");
builder.Services.AddSingleton(sp => _confMQTT);

builder.Services
    .Configure<RabbitMQConfigModel>(builder.Configuration.GetSection(RabbitMQConfigModel.Configuration))
    .Configure<RestApiConfigBaseModel>(builder.Configuration.GetSection(RestApiConfigBaseModel.Configuration))
    .Configure<PartUploadSessionConfigModel>(builder.Configuration.GetSection(PartUploadSessionConfigModel.Configuration))
;

builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();

string connectionNLogsString = builder.Configuration.GetConnectionString($"NLogsConnection{_modePrefix}") ?? throw new InvalidOperationException($"Connection string 'NLogsConnection{_modePrefix}' not found.");
builder.Services.AddDbContextFactory<NLogsContext>(opt =>
    opt.UseNpgsql(connectionNLogsString));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString($"RedisConnectionString{_modePrefix}");
})
.AddSingleton<IManualCustomCacheService, ManualCustomCacheService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<UnhandledExceptionAttribute>();
builder.Services
    .AddSingleton<ITraceRabbitActionsServiceTransmission, TraceRabbitActionsTransmission>()
    .AddScoped<IIndexingServive, IndexingTransmissionRabbit>()
    .AddScoped<IHistoryIndexing, HistoryTransmissionRabbit>()
    .AddScoped<RolesAuthorizationFilter>()
    ;

builder.Services.AddScoped<ExpressUserPermissionModel>();

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
    .AddScoped<IWebChatService, WebChatTransmissionRabbit>()
    .AddScoped<IWebTransmission, WebTransmissionRabbit>()
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
    ;
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
    metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
});

// Add Tracing for ASP.NET Core and our custom ActivitySource and export via OTLP
otel.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddHttpClientInstrumentation();
    tracing.AddSource($"OTel.{appName}");
});

builder.Services.AddScoped<IServerToolsService, ToolsSystemService>();

builder.Services
    .AddControllers(options => options.Filters.Add<LoggerActionFilter>())
    .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; options.SerializerSettings.Converters.Add(new StringEnumConverter() { }); });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.LoginPath = new PathString("/auth/login");
            options.AccessDeniedPath = new PathString("/auth/denied");
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
RestApiConfigBaseModel restConf = builder.Configuration.GetSection("ApiAccess").Get<RestApiConfigBaseModel>() ?? throw new Exception("Отсутствует конфигурация ApiAccess");
builder.Services.AddSwaggerGen(options =>
          {
              options.SwaggerDoc("v1", new OpenApiInfo
              {
                  Version = "v1",
                  Title = "tools - REST API",
                  Description = $"Удалённый RESTful доступ к <a href='https://github.com/badhitman/BlankCRM'>https://github.com/badhitman/BlankCRM</a>. Токен доступа следует передавать с каждым запросом через заголовок с именем '{restConf.TokenAccessHeaderName}'",
                  Contact = new OpenApiContact
                  {
                      Email = "ru.usa@mail.ru",
                      Url = new("https://github.com/badhitman"),
                      Name = "badhitman"
                  }
              });

              options.UseInlineDefinitionsForEnums();
              options.IncludeXmlComments(GetXmlCommentsPath(), includeControllerXmlComments: true);

              string commentsFileName = nameof(SharedLib) + ".xml";
              string baseDirectory = AppContext.BaseDirectory;
              options.IncludeXmlComments(Path.Combine(baseDirectory, commentsFileName), includeControllerXmlComments: true);
          });
builder.Services.AddSwaggerGenNewtonsoftSupport();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();
app.UseStaticFiles();
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", "tools - Rest/API");
    options.InjectStylesheet("/assets/css/swagger-style.css");
    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
});

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseForwardedHeaders();
app.MapControllers();
#if DEBUG
app.UseDeveloperExceptionPage();
#else
app.UseExceptionHandler("/Error");
#endif

//app.UseAuthentication();
//app.UseWhen(context =>
//{
//    return context.Request.Path.Value?.StartsWith("/api") ?? false;
//}, appBuilder =>
//{
//    app.UseMiddleware<PassageMiddleware>();
//});

//app.UseAuthorization();
app.Run();

static string GetXmlCommentsPath()
{
    string baseDirectory = AppContext.BaseDirectory;
    string commentsFileName = Assembly.GetEntryAssembly()!.GetName().Name + ".xml";
    return Path.Combine(baseDirectory, commentsFileName);
}