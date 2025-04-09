////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Transmission.Receives.Outers.Breez;
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
            .RegisterMqListener<DownloadAndSaveReceive, object, ResponseBaseModel>()
            .RegisterMqListener<LeftoversGetReceive, string, TResponseModel<List<BreezRuLeftoverModel>>>()
            .RegisterMqListener<HealthCheckReceive, object, TResponseModel<List<RabbitMqManagementResponseModel>>>()
            .RegisterMqListener<GetTechProductReceive, TechRequestModel, TResponseModel<List<TechProductBreezRuResponseModel>>>()
            .RegisterMqListener<GetTechCategoryReceive, TechRequestModel, TResponseModel<List<TechCategoryBreezRuModel>>>()
            .RegisterMqListener<GetProductsReceive, object, TResponseModel<List<ProductBreezRuModel>>>()
            .RegisterMqListener<GetCategoriesReceive, object, TResponseModel<List<CategoryRealBreezRuModel>>>()
            .RegisterMqListener<GetBrandsReceive, object, TResponseModel<List<BrandRealBreezRuModel>>>()
            ;
    }
}