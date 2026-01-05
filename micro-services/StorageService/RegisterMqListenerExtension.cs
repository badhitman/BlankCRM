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
            .RegisterMqListener<SaveParameterReceive, StorageCloudParameterPayloadModel, TResponseModel<int?>>()
            .RegisterMqListener<ReadParameterReceive, StorageMetadataModel, TResponseModel<StorageCloudParameterPayloadModel>>()
            .RegisterMqListener<ReadParametersReceive, StorageMetadataModel[], TResponseModel<List<StorageCloudParameterPayloadModel>>>()
            .RegisterMqListener<FindParametersReceive, FindStorageBaseModel, TResponseModel<FoundParameterModel[]>>()

            .RegisterMqListener<TagSetReceive, TagSetModel, ResponseBaseModel>()
            .RegisterMqListener<TagsSelectReceive, TPaginationRequestStandardModel<SelectMetadataRequestModel>, TPaginationResponseStandardModel<TagViewModel>>()

            .RegisterMqListener<GoToPageForRowReceive, TPaginationRequestStandardModel<int>, TPaginationResponseStandardModel<NLogRecordModelDB>>()
            .RegisterMqListener<MetadataLogsReceive, PeriodDatesTimesModel, TResponseModel<LogsMetadataResponseModel>>()
            .RegisterMqListener<LogsSelectReceive, TPaginationRequestStandardModel<LogsSelectRequestModel>, TPaginationResponseStandardModel<NLogRecordModelDB>>()
            .RegisterMqListener<SetWebConfigReceive, WebConfigModel, ResponseBaseModel>()

            .RegisterMqListener<SaveFileReceive, TAuthRequestStandardModel<StorageFileMetadataModel>, TResponseModel<StorageFileModelDB>>()
            .RegisterMqListener<ReadFileReceive, TAuthRequestStandardModel<RequestFileReadModel>, TResponseModel<FileContentModel>>()
            .RegisterMqListener<FilesAreaGetMetadataReceive, FilesAreaMetadataRequestModel, TResponseModel<FilesAreaMetadataModel[]>>()
            .RegisterMqListener<FilesSelectReceive, TPaginationRequestStandardModel<SelectMetadataRequestModel>, TPaginationResponseStandardModel<StorageFileModelDB>>()
            .RegisterMqListener<GetDirectoryInfoReceive, DirectoryReadRequestModel, TResponseModel<DirectoryReadResponseModel>>()

            ;
    }
}