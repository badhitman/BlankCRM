////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using Transmission.Receives.Outers.HaierProff;

namespace FeedsHaierProffRuService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection FeedsHaierProffRuRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<DownloadAndSaveReceive, object, ResponseBaseModel>()
            .RegisterMqListener<ProductsFeedGetReceive, object, TResponseModel<List<FeedItemHaierModel>>>()
            ;
    }
}