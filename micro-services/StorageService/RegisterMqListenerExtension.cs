////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using SharedLib;
using Transmission.Receives.storage;

namespace StorageService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection StorageRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterListenerRabbitMQ<SaveParameterReceive, StorageCloudParameterPayloadModel, TResponseModel<int?>>()
            .RegisterListenerRabbitMQ<ReadParameterReceive, StorageMetadataModel, TResponseModel<StorageCloudParameterPayloadModel>>()
            .RegisterListenerRabbitMQ<ReadParametersReceive, StorageMetadataModel[], TResponseModel<List<StorageCloudParameterPayloadModel>>>()
            .RegisterListenerRabbitMQ<FindParametersReceive, FindStorageBaseModel, TResponseModel<FoundParameterModel[]>>()

            .RegisterListenerRabbitMQ<TagSetReceive, TagSetModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<TagsSelectReceive, TPaginationRequestStandardModel<SelectMetadataRequestModel>, TPaginationResponseStandardModel<TagViewModel>>()

            .RegisterListenerRabbitMQ<GoToPageForRowReceive, TPaginationRequestStandardModel<GoToPageForRowLogsRequestModel>, TPaginationResponseStandardModel<NLogRecordModelDB>>()
            .RegisterListenerRabbitMQ<MetadataLogsReceive, PeriodDatesTimesModel, TResponseModel<LogsMetadataResponseModel>>()
            .RegisterListenerRabbitMQ<LogsSelectReceive, TPaginationRequestStandardModel<LogsSelectRequestModel>, TPaginationResponseStandardModel<NLogRecordModelDB>>()
            .RegisterListenerRabbitMQ<SetWebConfigReceive, WebConfigModel, ResponseBaseModel>()

            .RegisterListenerRabbitMQ<SaveFileReceive, TAuthRequestStandardModel<StorageFileMetadataModel>, TResponseModel<StorageFileModelDB>>()
            .RegisterListenerRabbitMQ<ReadFileReceive, TAuthRequestStandardModel<RequestFileReadModel>, TResponseModel<FileContentModel>>()
            .RegisterListenerRabbitMQ<FilesAreaGetMetadataReceive, FilesAreaMetadataRequestModel, TResponseModel<FilesAreaMetadataModel[]>>()
            .RegisterListenerRabbitMQ<FilesSelectReceive, TPaginationRequestStandardModel<SelectMetadataRequestModel>, TPaginationResponseStandardModel<StorageFileModelDB>>()
            .RegisterListenerRabbitMQ<GetDirectoryInfoReceive, DirectoryReadRequestModel, TResponseModel<DirectoryReadResponseModel>>()
            .RegisterListenerRabbitMQ<ReadFileDataAboutPositionReceive, ReadFileDataAboutPositionRequestModel, TResponseModel<Dictionary<DirectionsEnum, byte[]>>>()
            ;
    }
}