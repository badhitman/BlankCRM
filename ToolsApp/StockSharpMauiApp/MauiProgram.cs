////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using DbcLib;
using MudBlazor.Services;
using SharedLib;
using Transmission.Receives.StockSharpClient;
using Microsoft.EntityFrameworkCore;

namespace StockSharpMauiApp;

public static class MauiProgram
{
    /// <summary>
    /// db Path
    /// </summary>
    public static string DbPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(StockSharpAppContext), $"{(AppDomain.CurrentDomain.FriendlyName.Equals("ef", StringComparison.OrdinalIgnoreCase) ? "StockSharpAppData" : AppDomain.CurrentDomain.FriendlyName)}.db3");

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();

        FileInfo _fi = new(DbPath);
        if (_fi.Directory?.Exists != true)
            Directory.CreateDirectory(Path.GetDirectoryName(DbPath)!);

        builder.Services.AddDbContextFactory<StockSharpAppContext>(opt =>
        {
#if DEBUG
            opt.EnableSensitiveDataLogging(true);
//            opt.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
//            opt.UseSqlite(DbPath, b => b.MigrationsAssembly("StockSharpMauiMigration"));
#endif
        });

        StockSharpClientConfigModel _conf = StockSharpClientConfigModel.BuildEmpty();
        builder.Services.AddSingleton(sp => _conf);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        string appName = typeof(MauiProgram).Assembly.GetName().Name ?? "StockSharpMauiAppDemoAssemblyName";
        #region MQ Transmission (remote methods call)
        builder.Services.AddSingleton<IMQTTClient>(x => new MQttClient(x.GetRequiredService<StockSharpClientConfigModel>(), x.GetRequiredService<ILogger<MQttClient>>(), appName));
        //
        builder.Services
            .AddScoped<IStockSharpMainService, StockSharpMainService>()
            ;
        
        #endregion
        return builder.Build();
    }
}