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
            .RegisterListenerRabbitMQ<KladrNavigationListReceive, KladrsRequestBaseModel, Dictionary<KladrChainTypesEnum, JObject[]>>()
            .RegisterListenerRabbitMQ<UploadPartTempKladrReceive, UploadPartTableDataModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetMetadataKladrReceive, GetMetadataKladrRequestModel, MetadataKladrModel>()
            .RegisterListenerRabbitMQ<ClearTempKladrReceive, object, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<FlushTempKladrReceive, object, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ChildsContainsReceive, string, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ObjectGetReceive, KladrsRequestBaseModel, TResponseModel<KladrResponseModel>>()
            .RegisterListenerRabbitMQ<KladrSelectReceive, KladrSelectRequestModel, TPaginationResponseStandardModel<KladrResponseModel>>()
            .RegisterListenerRabbitMQ<KladrFindReceive, KladrFindRequestModel, TPaginationResponseStandardModel<KladrResponseModel>>()
            ;
    }
}