////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using RemoteCallLib;
using SharedLib;

namespace ZeroMQCallLib;

public static class ServiceProviderExtensions
{
    /// <summary>
    /// Register Mq Listener
    /// </summary>
    public static IServiceCollection RegisterMqListener<TQueue, TRequest, TResponse>(this IServiceCollection sc)
        where TQueue : class, IZeroMQReceive<TRequest?, TResponse?>
        where TResponse : class, new()
    {
        
        sc.AddScoped<IZeroMQReceive<TRequest?, TResponse?>, TQueue>();
        sc.AddHostedService<ZeroMQListenerService<TQueue, TRequest?, TResponse?>>();

        return sc;
    }

}
