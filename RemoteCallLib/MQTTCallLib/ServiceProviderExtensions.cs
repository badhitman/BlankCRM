////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using RemoteCallLib;
using SharedLib;

namespace MQTTCallLib;

/// <summary>
/// ServiceProviderExtensions
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Register Mq Listener
    /// </summary>
    public static IServiceCollection RegisterMqListener<TQueue, TRequest, TResponse>(this IServiceCollection sc)
        where TQueue : class, IMQTTReceive<TRequest?, TResponse?>
        where TResponse : class, new()
    {
        
        sc.AddScoped<IMQTTReceive<TRequest?, TResponse?>, TQueue>();
        sc.AddHostedService<MQTTListenerService<TQueue, TRequest?, TResponse?>>();

        return sc;
    }

    /// <summary>
    /// Register Mq Listener
    /// </summary>
    public static IServiceCollection RegisterMqListenerCli<TQueue, TRequest, TResponse>(this IServiceCollection sc)
        where TQueue : class, IMQTTReceive<TRequest?, TResponse?>
        where TResponse : class, new()
    {
        sc.AddScoped<IMQTTReceive<TRequest?, TResponse?>, TQueue>();
        sc.AddScoped<MQTTListenerService<TQueue, TRequest?, TResponse?>>();

        return sc;
    }
}