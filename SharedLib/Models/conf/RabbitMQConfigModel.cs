﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RabbitMQ configuration
/// </summary>
public class RabbitMQConfigModel
{
    /// <inheritdoc/>
    public static readonly string Configuration = "RabbitMQConfig";

    /// <inheritdoc/>
    public required string UserName { get; set; } = "guest";

    /// <inheritdoc/>
    public required string Password { get; set; } = "guest";

    /// <inheritdoc/>
    public required string VirtualHost { get; set; } = "/";

    /// <inheritdoc/>
    public required string HostName { get; set; } = "localhost";

    /// <inheritdoc/>
    public required int Port { get; set; } = 5672;

    /// <inheritdoc/>
    public required int PortManagementPlugin { get; set; } = 15672;

    /// <inheritdoc/>
    public required string ClientProvidedName { get; set; } = "guest-client";

    /// <summary>
    /// Таймаут ожидания ответа на удалённый вызов
    /// </summary>
    public int RemoteCallTimeoutMs { get; set; } = 3600000;


    /// <summary>
    /// Префикс имён очередей для ответов на удалённые команды
    /// </summary>
    public string QueueMqNamePrefixForResponse { get; set; } = "response.transit-";
}