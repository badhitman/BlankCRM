////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OrderReportGetReceive
/// </summary>
public class OrderReportGetReceive(ICommerceService commRepo) : IResponseReceive<TAuthRequestModel<int>?, TResponseModel<FileAttachModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrderReportGetCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<FileAttachModel>?> ResponseHandleActionAsync(TAuthRequestModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.GetOrderReportFileAsync(req, token);
    }
}