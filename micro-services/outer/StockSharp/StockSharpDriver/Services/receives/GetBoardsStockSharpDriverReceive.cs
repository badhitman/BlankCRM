////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Transmission.Receives.StockSharpDriver;

/// <summary>
/// GetBoardsStockSharpDriverReceive
/// </summary>
public class GetBoardsStockSharpDriverReceive(IStockSharpDriverService ssRepo)
    : IMQTTReceive<int[], TResponseModel<List<BoardStockSharpModel>>>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetBoardsStockSharpReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BoardStockSharpModel>>> ResponseHandleActionAsync(int[] req, CancellationToken token = default)
    {
        //if(req is null)
        //    throw new ArgumentNullException(nameof(req));

        return await ssRepo.GetBoardsAsync(req, token);
    }
}