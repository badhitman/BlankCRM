﻿////////////////////////////////////////////////
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
            .RegisterMqListener<GetTechProductReceive, TechRequestModel, TResponseModel<List<TechProductRealBreezRuModel>>>()
            .RegisterMqListener<GetTechCategoryReceive, TechRequestModel, TResponseModel<List<TechCategoryRealBreezRuModel>>>()
            .RegisterMqListener<GetProductsReceive, object, TResponseModel<List<ProductRealBreezRuModel>>>()
            .RegisterMqListener<GetCategoriesReceive, object, TResponseModel<List<CategoryRealBreezRuModel>>>()
            .RegisterMqListener<GetBrandsReceive, object, TResponseModel<List<BrandRealBreezRuModel>>>()
            ;
    }
}