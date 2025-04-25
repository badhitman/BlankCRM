namespace Telegram.Bot.Abstract;

/// <summary>
/// A marker interface for Update Receiver service
/// </summary>
public interface IReceiverService
{
    /// <inheritdoc/>
    Task ReceiveAsync(CancellationToken stoppingToken);
}
