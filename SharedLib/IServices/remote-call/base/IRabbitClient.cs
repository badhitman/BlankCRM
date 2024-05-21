﻿namespace SharedLib;

/// <summary>
/// RabbitMq client
/// </summary>
public interface IRabbitClient
{
    /// <summary>
    /// Удалённый вызов метода через MQ
    /// </summary>
    public Task<TResponseModel<T?>> MqRemoteCall<T>(string queue, object? request = null);
}