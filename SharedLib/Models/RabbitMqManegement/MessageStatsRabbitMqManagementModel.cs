namespace SharedLib;

/// <inheritdoc/>
public class MessageStatsRabbitMqManagementsModel
{
    /// <inheritdoc/>
    public int ack { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? ack_details { get; set; }
    
    /// <inheritdoc/>
    public int deliver { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? deliver_details { get; set; }
    
    /// <inheritdoc/>
    public int deliver_get { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? deliver_get_details { get; set; }
    
    /// <inheritdoc/>
    public int deliver_no_ack { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? deliver_no_ack_details { get; set; }
    
    /// <inheritdoc/>
    public int get { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? get_details { get; set; }
    
    /// <inheritdoc/>
    public int get_empty { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? get_empty_details { get; set; }
    
    /// <inheritdoc/>
    public int get_no_ack { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? get_no_ack_details { get; set; }
    
    /// <inheritdoc/>
    public int publish { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? publish_details { get; set; }
    
    /// <inheritdoc/>
    public int redeliver { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManagementModel? redeliver_details { get; set; }
}