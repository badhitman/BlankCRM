////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Transmission.Receives.Outers.Rusklimat;
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
            .RegisterMqListener<DownloadAndSaveReceive, object, ResponseBaseModel>()
            .RegisterMqListener<GetCategoriesReceive, object, CategoriesRusklimatResponseModel>()
            .RegisterMqListener<GetProductsReceive, PaginationRequestModel, ProductsRusklimatResponseModel>()
            .RegisterMqListener<GetPropertiesReceive, object, PropertiesRusklimatResponseModel>()
            .RegisterMqListener<GetUnitsReceive, object, UnitsRusklimatResponseModel>()
            ;
    }
}