////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ProxyNetMQConfig
/// </summary>
public class ProxyNetMQConfigModel
{
    /// <inheritdoc/>
    public required HostConfigModel PublisherSocketEndpoint { get; set; }
    /// <inheritdoc/>
    public required HostConfigModel SubscriberSocketEndpoint { get; set; }

    /// <summary>
    /// Шаблоны MQ очередей для трассировки
    /// </summary>
    public static ProxyNetMQConfigModel BuildEmpty()
    {
        return new ProxyNetMQConfigModel()
        {
            PublisherSocketEndpoint = new()
            {
                Scheme = "tcp",
                Port = 2883,
                Host = "127.0.0.1"
            },

            SubscriberSocketEndpoint = new()
            {
                Scheme = "tcp",
                Port = 2884,
                Host = "127.0.0.1"
            },
        };
    }

    /// <inheritdoc/>
    public List<string>? TracesNamesPatterns { get; set; }

    /// <inheritdoc/>
    public void Reload(ProxyNetMQConfigModel other)
    {
        PublisherSocketEndpoint.Reload(other.PublisherSocketEndpoint);
        SubscriberSocketEndpoint.Reload(other.SubscriberSocketEndpoint);
        //Host = other.Host;
        //Port = other.Port;
    }

    /// <inheritdoc/>
    public static readonly string Configuration = "ProxyNetMQConfig";
}