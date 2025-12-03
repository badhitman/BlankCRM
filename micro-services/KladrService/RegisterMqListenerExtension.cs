////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using SharedLib;
using Transmission.Receives.kladr;

namespace KladrService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection KladrRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<KladrNavigationListReceive, KladrsRequestBaseModel, Dictionary<KladrChainTypesEnum, JObject[]>>()
            .RegisterMqListener<UploadPartTempKladrReceive, UploadPartTableDataModel, ResponseBaseModel>()
            .RegisterMqListener<GetMetadataKladrReceive, GetMetadataKladrRequestModel, MetadataKladrModel>()
            .RegisterMqListener<ClearTempKladrReceive, object, ResponseBaseModel>()
            .RegisterMqListener<FlushTempKladrReceive, object, ResponseBaseModel>()
            .RegisterMqListener<ChildsContainsReceive, string, ResponseBaseModel>()
            .RegisterMqListener<ObjectGetReceive, KladrsRequestBaseModel, TResponseModel<KladrResponseModel>>()
            .RegisterMqListener<KladrSelectReceive, KladrSelectRequestModel, TPaginationResponseModel<KladrResponseModel>>()
            .RegisterMqListener<KladrFindReceive, KladrFindRequestModel, TPaginationResponseModel<KladrResponseModel>>()
            ;
    }
}