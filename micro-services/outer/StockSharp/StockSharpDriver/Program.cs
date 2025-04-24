////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;
using StockSharpService;

namespace StockSharpDriver;

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
        IHostBuilder builderH = Host.CreateDefaultBuilder(args);

        string curr_dir = Directory.GetCurrentDirectory();
        string _environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
        string appName = typeof(Program).Assembly.GetName().Name ?? "StockSharpDriverAssemblyNameDemo";

        builderH
            .ConfigureAppConfiguration((bx, builder) =>
            {
                builder.SetBasePath(curr_dir);

                string path_load = Path.Combine(curr_dir, "appsettings.json");
                if (File.Exists(path_load))
                    builder.AddJsonFile(path_load, optional: true, reloadOnChange: true);

                path_load = Path.Combine(curr_dir, $"appsettings.{_environmentName}.json");
                if (File.Exists(path_load))
                    builder.AddJsonFile(path_load, optional: true, reloadOnChange: true);

                // Secrets
                void ReadSecrets(string dirName)
                {
                    string secretPath = Path.Combine("..", dirName);
                    DirectoryInfo di = new(secretPath);
                    for (int i = 0; i < 5 && !di.Exists; i++)
                    {
                        secretPath = Path.Combine("..", secretPath);
                        di = new(secretPath);
                    }

                    if (Directory.Exists(secretPath))
                    {
                        foreach (string secret in Directory.GetFiles(secretPath, $"*.json"))
                        {
                            path_load = Path.GetFullPath(secret);
                            builder.AddJsonFile(path_load, optional: true, reloadOnChange: true);
                        }
                    }
                }
                ReadSecrets("secrets");

                builder.AddEnvironmentVariables();
                builder.AddCommandLine(args);
                builder.Build();
                //_conf.Reload(bx.Configuration.GetValue<StockSharpClientConfigModel>("StockSharpDriverConfig"));
            })
            .ConfigureServices((bx, services) =>
            {
                _conf.Reload(bx.Configuration.GetSection("StockSharpDriverConfig").Get<StockSharpClientConfigModel>());
                services.AddHostedService<Worker>();
                services.AddSingleton(sp => _conf);

                #region MQ Transmission (remote methods call)
                services.AddSingleton<IMQTTClient>(x => new MQttClient(x.GetRequiredService<StockSharpClientConfigModel>(), x.GetRequiredService<ILogger<MQttClient>>(), appName));
                //
                services
                .AddScoped<IStockSharpMainService, StockSharpMainServiceTransmission>()
                .AddScoped<IStockSharpDriverService, StockSharpDriverService>()
                ;

                services.StockSharpRegisterMqListeners();
                #endregion                 
            });

        IHost host = builderH.Build();
        
        host.Run();
    }
}