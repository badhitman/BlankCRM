////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using DbcLib;
using MudBlazor.Services;
using SharedLib;
using StockSharpService;
using RemoteCallLib;

namespace StockSharpMauiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();

            builder.Services.AddDbContextFactory<StockSharpAppContext>(opt =>
            {
#if DEBUG
                opt.EnableSensitiveDataLogging(true);
                opt.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
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
            builder.Services.AddSingleton<IZeroMQClient>(x => new ZeroMQClient(x.GetRequiredService<StockSharpClientConfigModel>(), x.GetRequiredService<ILogger<ZeroMQClient>>(), appName));
            //
            builder.Services.AddScoped<IStockSharpDriverService, StockSharpDriverTransmission>();
            //
            builder.Services.StockSharpRegisterMqListeners();
            #endregion
            return builder.Build();
        }
    }
}