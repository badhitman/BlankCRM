////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Transmission.Receives.Outers.Daichi;
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
            .RegisterMqListener<StoresGetReceive, object?, TResponseModel<StoresDaichiBusinessResponseModel>>()
            .RegisterMqListener<ProductsParamsGetReceive, ProductParamsRequestDaichiModel, TResponseModel<ProductsParamsDaichiBusinessResponseModel>>()
            .RegisterMqListener<ProductsGetReceive, ProductsRequestDaichiModel, TResponseModel<ProductsDaichiBusinessResultModel>>()
            .RegisterMqListener<DownloadAndSaveReceive, object, ResponseBaseModel>()
            ;
    }
}