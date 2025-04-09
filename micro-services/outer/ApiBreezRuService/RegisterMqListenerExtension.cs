////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Transmission.Receives.Outers.Breez;
using SharedLib;
using System.Collections.Generic;

namespace ApiBreezRuService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection ApiBreezRuRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<DownloadAndSaveReceive, object, ResponseBaseModel>()
            .RegisterMqListener<LeftoversGetReceive, string, TResponseModel<List<BreezRuGoodsModel>>>()
            .RegisterMqListener<HealthCheckReceive, object, TResponseModel<List<RabbitMqManagementResponseModel>>>()
            ;
    }
}