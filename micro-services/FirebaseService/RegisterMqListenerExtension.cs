////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Transmission.Receives.firebase;
using SharedLib;

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
            .RegisterListenerRabbitMQ<SendFirebaseMessageReceive, TAuthRequestStandardModel<SendFirebaseMessageRequestModel>, TResponseModel<SendFirebaseMessageResultModel>>()
            ;
    }
}