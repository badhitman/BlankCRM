////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using SharedLib;


namespace FeedsHaierproffRuService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection FeedsHaierproffRuRegisterMqListeners(this IServiceCollection services)
    {
        return services
            //.RegisterMqListener<, , >()
            //.RegisterMqListener<, , >()
            ;
    }
}