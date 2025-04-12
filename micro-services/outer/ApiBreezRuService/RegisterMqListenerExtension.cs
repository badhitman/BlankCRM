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
            .RegisterMqListener<GetTechProductReceive, TechRequestBreezModel, TResponseModel<List<TechProductRealBreezRuModel>>>()
            .RegisterMqListener<GetTechCategoryReceive, TechRequestBreezModel, TResponseModel<List<TechCategoryRealBreezRuModel>>>()
            .RegisterMqListener<GetProductsReceive, object, TResponseModel<List<ProductRealBreezRuModel>>>()
            .RegisterMqListener<GetCategoriesReceive, object, TResponseModel<List<CategoryRealBreezRuModel>>>()
            .RegisterMqListener<GetBrandsReceive, object, TResponseModel<List<BrandRealBreezRuModel>>>()
            .RegisterMqListener<ProductUpdateReceive, ProductBreezRuModelDB, ResponseBaseModel>()
            .RegisterMqListener<TechProductUpdateReceive, TechProductBreezRuModelDB, ResponseBaseModel>()
            .RegisterMqListener<ProductsSelectReceive, BreezRequestModel, TPaginationResponseModel<ProductBreezRuModelDB>>()
            ;
    }
}