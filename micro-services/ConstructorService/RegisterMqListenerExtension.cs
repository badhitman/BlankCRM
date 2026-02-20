////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Transmission.Receives.constructor;
using SharedLib;

namespace ConstructorService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection ConstructorRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterListenerRabbitMQ<CreateProjectConstructorReceive, CreateProjectRequestModel, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<ProjectsReadConstructorReceive, int[], List<ProjectModelDb>>()
            .RegisterListenerRabbitMQ<CheckAndNormalizeSortIndexForElementsOfDirectoryConstructorReceive, int, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<AddRowToTableConstructorReceive, FieldSessionDocumentDataBaseModel, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<DeleteValuesFieldsByGroupSessionDocumentDataByRowNumConstructorReceive, ValueFieldSessionDocumentDataBaseModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<SetDoneSessionDocumentDataConstructorReceive, string, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<SetValueFieldSessionDocumentDataConstructorReceive, SetValueFieldDocumentDataModel, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterListenerRabbitMQ<GetSessionDocumentDataConstructorReceive, string, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterListenerRabbitMQ<SelectFormsConstructorReceive, SelectFormsModel, TPaginationResponseStandardModel<FormConstructorModelDB>>()
            .RegisterListenerRabbitMQ<DeleteSessionDocumentConstructorReceive, DeleteSessionDocumentRequestModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ClearValuesForFieldNameConstructorReceive, FormFieldOfSessionModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<FindSessionsDocumentsByFormFieldNameConstructorReceive, FormFieldModel, TResponseModel<EntryDictStandardModel[]>>()
            .RegisterListenerRabbitMQ<RequestSessionsDocumentsConstructorReceive, RequestSessionsDocumentsRequestPaginationModel, TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>>()
            .RegisterListenerRabbitMQ<UpdateOrCreateSessionDocumentConstructorReceive, SessionOfDocumentDataModelDB, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterListenerRabbitMQ<GetSessionDocumentConstructorReceive, SessionGetModel, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterListenerRabbitMQ<SetStatusSessionDocumentConstructorReceive, SessionStatusModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<SaveSessionFormConstructorReceive, SaveConstructorSessionRequestModel, TResponseModel<ValueDataForSessionOfDocumentModelDB[]>>()
            .RegisterListenerRabbitMQ<DeleteTabDocumentSchemeJoinFormConstructorReceive, TAuthRequestStandardModel<DeleteTabDocumentSchemeJoinFormRequestModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<MoveTabDocumentSchemeJoinFormConstructorReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<TabOfDocumentSchemeConstructorModelDB>>()
            .RegisterListenerRabbitMQ<CreateOrUpdateTabDocumentSchemeJoinFormConstructorReceive, TAuthRequestStandardModel<FormToTabJoinConstructorModelDB>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetTabDocumentSchemeJoinFormConstructorReceive, int, TResponseModel<FormToTabJoinConstructorModelDB?>>()
            .RegisterListenerRabbitMQ<DeleteTabOfDocumentSchemeConstructorReceive, TAuthRequestStandardModel<DeleteTabOfDocumentSchemeRequestModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetTabOfDocumentSchemeConstructorReceive, int, TResponseModel<TabOfDocumentSchemeConstructorModelDB?>>()
            .RegisterListenerRabbitMQ<MoveTabOfDocumentSchemeConstructorReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<DocumentSchemeConstructorModelDB>>()
            .RegisterListenerRabbitMQ<CreateOrUpdateTabOfDocumentSchemeConstructorReceive, TAuthRequestStandardModel<EntryDescriptionOwnedModel>, TResponseModel<TabOfDocumentSchemeConstructorModelDB>>()
            .RegisterListenerRabbitMQ<DeleteDocumentSchemeConstructorReceive, TAuthRequestStandardModel<DeleteDocumentSchemeRequestModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetDocumentSchemeConstructorReceive, int, TResponseModel<DocumentSchemeConstructorModelDB?>>()
            .RegisterListenerRabbitMQ<RequestDocumentsSchemesConstructorReceive, RequestDocumentsSchemesModel, TPaginationResponseStandardModel<DocumentSchemeConstructorModelDB>>()
            .RegisterListenerRabbitMQ<UpdateOrCreateDocumentSchemeConstructorReceive, TAuthRequestStandardModel<EntryConstructedModel>, TResponseModel<DocumentSchemeConstructorModelDB?>>()
            .RegisterListenerRabbitMQ<FormFieldDirectoryDeleteConstructorReceive, TAuthRequestStandardModel<FormFieldDirectoryDeleteRequestModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<FormFieldDirectoryUpdateOrCreateConstructorReceive, TAuthRequestStandardModel<FieldFormAkaDirectoryConstructorModelDB>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<FormFieldDeleteConstructorReceive, TAuthRequestStandardModel<FormFieldDeleteRequestModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<FormFieldUpdateOrCreateConstructorReceive, TAuthRequestStandardModel<FieldFormConstructorModelDB>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<FieldDirectoryFormMoveConstructorReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<FormConstructorModelDB>>()
            .RegisterListenerRabbitMQ<FieldFormMoveConstructorReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<FormConstructorModelDB>>()
            .RegisterListenerRabbitMQ<FormDeleteConstructorReceive, TAuthRequestStandardModel<FormDeleteRequestModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<FormUpdateOrCreateConstructorReceive, TAuthRequestStandardModel<FormBaseConstructorModel>, TResponseModel<FormConstructorModelDB>>()
            .RegisterListenerRabbitMQ<GetFormConstructorReceive, int, TResponseModel<FormConstructorModelDB?>>()
            .RegisterListenerRabbitMQ<GetElementsOfDirectoryConstructorReceive, int, TResponseModel<List<EntryStandardModel>?>>()
            .RegisterListenerRabbitMQ<CreateElementForDirectoryConstructorReceive, TAuthRequestStandardModel<OwnedNameModel>, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<UpdateElementOfDirectoryConstructorReceive, TAuthRequestStandardModel<EntryDescriptionModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetElementOfDirectoryConstructorReceive, int, TResponseModel<EntryDescriptionModel>>()
            .RegisterListenerRabbitMQ<DeleteElementFromDirectoryConstructorReceive, TAuthRequestStandardModel<DeleteElementFromDirectoryRequestModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<UpMoveElementOfDirectoryConstructorReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<DownMoveElementOfDirectoryConstructorReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<DeleteDirectoryConstructorReceive, TAuthRequestStandardModel<DeleteDirectoryRequestModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<UpdateOrCreateDirectoryConstructorReceive, TAuthRequestStandardModel<EntryConstructedModel>, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<GetDirectoryConstructorReceive, int, TResponseModel<EntryDescriptionModel>>()
            .RegisterListenerRabbitMQ<GetDirectoriesConstructorReceive, ProjectFindModel, TResponseModel<EntryStandardModel[]>>()
            .RegisterListenerRabbitMQ<ReadDirectoriesConstructorReceive, int[], List<EntryNestedModel>>()
            .RegisterListenerRabbitMQ<GetCurrentMainProjectConstructorReceive, string, TResponseModel<MainProjectViewModel?>>()
            .RegisterListenerRabbitMQ<DeleteMembersFromProjectConstructorReceive, UsersProjectModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<CanEditProjectConstructorReceive, UserProjectModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<SetProjectAsMainConstructorReceive, UserProjectModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<AddMembersToProjectConstructorReceive, UsersProjectModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetMembersOfProjectConstructorReceive, int, TResponseModel<EntryAltStandardModel[]>>()
            .RegisterListenerRabbitMQ<UpdateProjectConstructorReceive, ProjectViewModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<SetMarkerDeleteProjectConstructorReceive, SetMarkerProjectRequestModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetProjectsForUserConstructorReceive, GetProjectsForUserRequestModel, TResponseModel<ProjectViewModel[]>>()
            ;
    }
}