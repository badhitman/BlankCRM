////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using SharedLib;


namespace ApiRusklimatComService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection ApiRusklimatComRegisterMqListeners(this IServiceCollection services)
    {
        return services
            //.RegisterMqListener<, , >()
            //.RegisterMqListener<, , >()
            ;
    }
}