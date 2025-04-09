using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <inheritdoc/>
public class RabbitMqManagementResponseModel
{
    /// <inheritdoc/>
    public JObject? arguments { get; set; }
    
    /// <inheritdoc/>
    public bool auto_delete { get; set; }
    
    /// <inheritdoc/>
    public double consumer_capacity { get; set; }
    
    /// <inheritdoc/>
    public double consumer_utilisation { get; set; }
    
    /// <inheritdoc/>
    public int consumers { get; set; }
    
    /// <inheritdoc/>
    public bool durable { get; set; }
    
    /// <inheritdoc/>
    public JObject? effective_policy_definition { get; set; }
    
    /// <inheritdoc/>
    public bool exclusive { get; set; }
   
    /// <inheritdoc/>
    public int memory { get; set; }
    
    /// <inheritdoc/>
    public int message_bytes { get; set; }
    
    /// <inheritdoc/>
    public int message_bytes_paged_out { get; set; }
    
    /// <inheritdoc/>
    public int message_bytes_persistent { get; set; }
    
    /// <inheritdoc/>
    public long message_bytes_ram { get; set; }
    
    /// <inheritdoc/>
    public int message_bytes_ready { get; set; }
    
    /// <inheritdoc/>
    public int message_bytes_unacknowledged { get; set; }
    
    /// <inheritdoc/>
    public int messages { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? messages_details { get; set; }
    
    /// <inheritdoc/>
    public int messages_paged_out { get; set; }
    
    /// <inheritdoc/>
    public int messages_persistent { get; set; }
    
    /// <inheritdoc/>
    public int messages_ram { get; set; }
    
    /// <inheritdoc/>
    public int messages_ready { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? messages_ready_details { get; set; }
    
    /// <inheritdoc/>
    public int messages_ready_ram { get; set; }
    
    /// <inheritdoc/>
    public int messages_unacknowledged { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? messages_unacknowledged_details { get; set; }
    
    /// <inheritdoc/>
    public int messages_unacknowledged_ram { get; set; }
    
    /// <inheritdoc/>
    public string? name { get; set; }
    
    /// <inheritdoc/>
    public string? node { get; set; }
    
    /// <inheritdoc/>
    public int reductions { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? reductions_details { get; set; }
    
    /// <inheritdoc/>
    public string? state { get; set; }
    
    /// <inheritdoc/>
    public int storage_version { get; set; }
    
    /// <inheritdoc/>
    public string? type { get; set; }
    
    /// <inheritdoc/>
    public string? vhost { get; set; }
    
    /// <inheritdoc/>
    public MessageStatsRabbitMqManagementsModel? message_stats { get; set; }
}