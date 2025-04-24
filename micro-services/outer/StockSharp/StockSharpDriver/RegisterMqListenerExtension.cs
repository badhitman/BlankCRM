////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using MQTTCallLib;
using Transmission.Receives.StockSharpDriver;

namespace StockSharpService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection StockSharpRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<PingStockSharpDriverReceive, object, ResponseBaseModel>()
            ;
    }
}