namespace SharedLib;

/// <inheritdoc/>
public class MessageStatsRabbitMqManegementModel
{
    /// <inheritdoc/>
    public int ack { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManegementModel? ack_details { get; set; }
    
    /// <inheritdoc/>
    public int deliver { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManegementModel? deliver_details { get; set; }
    
    /// <inheritdoc/>
    public int deliver_get { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManegementModel? deliver_get_details { get; set; }
    
    /// <inheritdoc/>
    public int deliver_no_ack { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManegementModel? deliver_no_ack_details { get; set; }
    
    /// <inheritdoc/>
    public int get { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManegementModel? get_details { get; set; }
    
    /// <inheritdoc/>
    public int get_empty { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManegementModel? get_empty_details { get; set; }
    
    /// <inheritdoc/>
    public int get_no_ack { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManegementModel? get_no_ack_details { get; set; }
    
    /// <inheritdoc/>
    public int publish { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManegementModel? publish_details { get; set; }
    
    /// <inheritdoc/>
    public int redeliver { get; set; }
    
    /// <inheritdoc/>
    public RateDetailsRabbitMqManegementModel? redeliver_details { get; set; }
}