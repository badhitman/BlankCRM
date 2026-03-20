////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using SharedLib;
using Transmission.Receives.firebase;

namespace FirebaseService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection FirebaseRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterListenerRabbitMQ<GetFirebaseConfigReceive, object, TResponseModel<FirebaseSDKConfigModel>>()
            ;
    }
}