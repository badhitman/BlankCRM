using RemoteCallLib;
using SharedLib;
using System.Text;

namespace LdapService;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        builder.Services
            .AddScoped<IIndexingServive, IndexingTransmission>()
            .AddScoped<IHistoryIndexing, HistoryTransmission>()
            .AddSingleton<ITraceRabbitActionsServiceTransmission, TraceRabbitActionsTransmission>()
            ;

        var host = builder.Build();
        host.Run();
    }
}