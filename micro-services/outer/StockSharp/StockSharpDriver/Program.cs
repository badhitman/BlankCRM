////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;
using StockSharpService;

namespace StockSharpJoinService;

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
        StockSharpClientConfigModel _conf = StockSharpClientConfigModel.BuildEmpty();
        string appName = typeof(Program).Assembly.GetName().Name ?? "StockSharpDriverDemoAssemblyName";
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                // services.AddHostedService<Worker>();
                services.AddSingleton(sp => _conf);

                #region MQ Transmission (remote methods call)
                services.AddSingleton<IZeroMQClient>(x => new ZeroMQClient(x.GetRequiredService<StockSharpClientConfigModel>(), x.GetRequiredService<ILogger<ZeroMQClient>>(), appName));
                //
                services.AddScoped<IStockSharpMainService, StockSharpMainServiceTransmission>();

                services.StockSharpRegisterMqListeners();
                #endregion                 
            })
            .Build();

        host.Run();
    }
}