////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Options;
using RemoteCallLib;
using SharedLib;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// ServiceProviderExtensions
/// </summary>
public static class ServiceProviderExtensionsZero
{
    /// <inheritdoc/>
    public static TResponseModel<object?>? SetRemoteConf = null;

    /// <summary>
    /// Register Mq Listener
    /// </summary>
    public static IServiceCollection RegisterListenerNetMQ<TQueue, TRequest, TResponse>(this IServiceCollection sc)
        where TQueue : class, IMQStandardReceive<TRequest?, TResponse?>
        where TResponse : class, new()
    {
        sc.AddScoped<IMQStandardReceive<TRequest?, TResponse?>, TQueue>();
        sc.AddHostedService<NetMQListenerService<TQueue, TRequest?, TResponse?>>();

        return sc;
    }
}