////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using Microsoft.Extensions.DependencyInjection;
using Transmission.Receives.StockSharp.Main;
using ZeroMQCallLib;

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
            .RegisterMqListener<PingSharpMainReceive, object, ResponseBaseModel>()
            ;
    }
}