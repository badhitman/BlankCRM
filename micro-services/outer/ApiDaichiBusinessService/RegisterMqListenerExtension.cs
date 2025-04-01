////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using SharedLib;


namespace ApiDaichiBusinessService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection ApiDaichiBusinessRegisterMqListeners(this IServiceCollection services)
    {
        return services
            //.RegisterMqListener<, , >()
            //.RegisterMqListener<, , >()
            ;
    }
}