////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace BankService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection BankRegisterMqListeners(this IServiceCollection services)
    {
        return services
            // .RegisterMqListener<OrganizationSetLegalReceive, OrganizationLegalModel, TResponseModel<bool>>()            
            ;
    }
}