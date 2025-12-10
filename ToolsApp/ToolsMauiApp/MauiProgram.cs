////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using CommunityToolkit.Maui;
using DbcLib;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using SharedLib;
using System.Net;
using System.Text;
using ToolsMauiLib;

namespace ToolsMauiApp;

/// <inheritdoc/>
public static class MauiProgram
{
    /// <inheritdoc/>
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        //
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        builder.Services.AddDbContextFactory<ToolsAppContext>(opt =>
        {
#if DEBUG
            opt.EnableSensitiveDataLogging(true);
            opt.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
#endif
        });

        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();

        ApiRestConfigModelDB _conf = ApiRestConfigModelDB.BuildEmpty();
        builder.Services.AddSingleton(sp => _conf);

        builder.Services.AddScoped<ILogsService, LogsService>();
        builder.Services.AddScoped<IToolsAppManager, ToolsAppManager>();
        builder.Services.AddTransient<ParseDBF>();

        builder.Services.AddScoped<IClientRestToolsService, ToolsSystemHTTPRestService>();
        builder.Services.AddScoped<IServerToolsService, ToolsSystemService>();

        builder.Services.AddHttpClient(HttpClientsNamesEnum.Kladr.ToString(), cc =>
        {
            cc.BaseAddress = new Uri(_conf.AddressBaseUri ?? "localhost");
            cc.DefaultRequestHeaders.Add(_conf.HeaderName, _conf.TokenAccess);
        })
#if DEBUG
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = delegate { return true; }
            };
        });
#endif
        // #if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        // #endif
        return builder.Build();
    }
}