////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using Transmission.Receives.StockSharpClient;
using MQTTCallLib;
using SharedLib;

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
            .RegisterMqListenerCli<PingSharpMainReceive, object, ResponseBaseModel>()
            ;
    }
}