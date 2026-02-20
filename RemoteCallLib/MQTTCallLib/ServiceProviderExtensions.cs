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
    public static IServiceCollection RegisterListenerMQTT<TQueue, TRequest, TResponse>(this IServiceCollection sc)
        where TQueue : class, IMQStandardReceive<TRequest?, TResponse?>
        where TResponse : class, new()
    {
        
        sc.AddScoped<IMQStandardReceive<TRequest?, TResponse?>, TQueue>();
        sc.AddHostedService<MQTTListenerService<TQueue, TRequest?, TResponse?>>();

        return sc;
    }
}