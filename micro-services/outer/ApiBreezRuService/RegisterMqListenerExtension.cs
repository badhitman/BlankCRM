////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using SharedLib;


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
            //.RegisterMqListener<, , >()
            //.RegisterMqListener<, , >()
            ;
    }
}