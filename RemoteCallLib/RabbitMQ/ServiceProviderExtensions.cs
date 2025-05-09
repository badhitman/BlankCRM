﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Options;
using RemoteCallLib;
using SharedLib;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// ServiceProviderExtensions
/// </summary>
public static class ServiceProviderExtensions
{
    /// <inheritdoc/>
    public static TResponseModel<object?>? SetRemoteConf = null;

    /// <summary>
    /// Получить конфигурацию
    /// </summary>
    public static T GetConfiguration<T>(this IServiceProvider serviceProvider)
        where T : class
    {
        IOptions<T>? o = serviceProvider.GetService<IOptions<T>>();
        return o is null ? throw new ArgumentNullException(nameof(T)) : o.Value;
    }

    /// <summary>
    /// Register Mq Listener
    /// </summary>
    public static IServiceCollection RegisterMqListener<TQueue, TRequest, TResponse>(this IServiceCollection sc)
        where TQueue : class, IResponseReceive<TRequest?, TResponse?>
        where TResponse : class, new()
    {
        sc.AddScoped<IResponseReceive<TRequest?, TResponse?>, TQueue>();
        sc.AddHostedService<RabbitMqListenerService<TQueue, TRequest?, TResponse?>>();

        return sc;
    }
}