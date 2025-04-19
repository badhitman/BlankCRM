////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using Transmission.Receives.StockSharp.Driver;
using MQTTCallLib;

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