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
            .RegisterMqListener<CreateProjectConstructorReceive, CreateProjectRequestModel, TResponseModel<int>>()
            .RegisterMqListener<ProjectsReadConstructorReceive, int[], List<ProjectModelDb>>()
            .RegisterMqListener<CheckAndNormalizeSortIndexForElementsOfDirectoryConstructorReceive, int, ResponseBaseModel>()
            .RegisterMqListener<AddRowToTableConstructorReceive, FieldSessionDocumentDataBaseModel, TResponseModel<int>>()
            .RegisterMqListener<DeleteValuesFieldsByGroupSessionDocumentDataByRowNumConstructorReceive, ValueFieldSessionDocumentDataBaseModel, ResponseBaseModel>()
            .RegisterMqListener<SetDoneSessionDocumentDataConstructorReceive, string, ResponseBaseModel>()
            .RegisterMqListener<SetValueFieldSessionDocumentDataConstructorReceive, SetValueFieldDocumentDataModel, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterMqListener<GetSessionDocumentDataConstructorReceive, string, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterMqListener<SelectFormsConstructorReceive, SelectFormsModel, TPaginationResponseStandardModel<FormConstructorModelDB>>()
            .RegisterMqListener<DeleteSessionDocumentConstructorReceive, DeleteSessionDocumentRequestModel, ResponseBaseModel>()
            .RegisterMqListener<ClearValuesForFieldNameConstructorReceive, FormFieldOfSessionModel, ResponseBaseModel>()
            .RegisterMqListener<FindSessionsDocumentsByFormFieldNameConstructorReceive, FormFieldModel, TResponseModel<EntryDictStandardModel[]>>()
            .RegisterMqListener<RequestSessionsDocumentsConstructorReceive, RequestSessionsDocumentsRequestPaginationModel, TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>>()
            .RegisterMqListener<UpdateOrCreateSessionDocumentConstructorReceive, SessionOfDocumentDataModelDB, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterMqListener<GetSessionDocumentConstructorReceive, SessionGetModel, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterMqListener<SetStatusSessionDocumentConstructorReceive, SessionStatusModel, ResponseBaseModel>()
            .RegisterMqListener<SaveSessionFormConstructorReceive, SaveConstructorSessionRequestModel, TResponseModel<ValueDataForSessionOfDocumentModelDB[]>>()
            .RegisterMqListener<DeleteTabDocumentSchemeJoinFormConstructorReceive, TAuthRequestStandardModel<DeleteTabDocumentSchemeJoinFormRequestModel>, ResponseBaseModel>()
            .RegisterMqListener<MoveTabDocumentSchemeJoinFormConstructorReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<TabOfDocumentSchemeConstructorModelDB>>()
            .RegisterMqListener<CreateOrUpdateTabDocumentSchemeJoinFormConstructorReceive, TAuthRequestStandardModel<FormToTabJoinConstructorModelDB>, ResponseBaseModel>()
            .RegisterMqListener<GetTabDocumentSchemeJoinFormConstructorReceive, int, TResponseModel<FormToTabJoinConstructorModelDB?>>()
            .RegisterMqListener<DeleteTabOfDocumentSchemeConstructorReceive, TAuthRequestStandardModel<DeleteTabOfDocumentSchemeRequestModel>, ResponseBaseModel>()
            .RegisterMqListener<GetTabOfDocumentSchemeConstructorReceive, int, TResponseModel<TabOfDocumentSchemeConstructorModelDB?>>()
            .RegisterMqListener<MoveTabOfDocumentSchemeConstructorReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<DocumentSchemeConstructorModelDB>>()
            .RegisterMqListener<CreateOrUpdateTabOfDocumentSchemeConstructorReceive, TAuthRequestStandardModel<EntryDescriptionOwnedModel>, TResponseModel<TabOfDocumentSchemeConstructorModelDB>>()
            .RegisterMqListener<DeleteDocumentSchemeConstructorReceive, TAuthRequestStandardModel<DeleteDocumentSchemeRequestModel>, ResponseBaseModel>()
            .RegisterMqListener<GetDocumentSchemeConstructorReceive, int, TResponseModel<DocumentSchemeConstructorModelDB?>>()
            .RegisterMqListener<RequestDocumentsSchemesConstructorReceive, RequestDocumentsSchemesModel, TPaginationResponseStandardModel<DocumentSchemeConstructorModelDB>>()
            .RegisterMqListener<UpdateOrCreateDocumentSchemeConstructorReceive, TAuthRequestStandardModel<EntryConstructedModel>, TResponseModel<DocumentSchemeConstructorModelDB?>>()
            .RegisterMqListener<FormFieldDirectoryDeleteConstructorReceive, TAuthRequestStandardModel<FormFieldDirectoryDeleteRequestModel>, ResponseBaseModel>()
            .RegisterMqListener<FormFieldDirectoryUpdateOrCreateConstructorReceive, TAuthRequestStandardModel<FieldFormAkaDirectoryConstructorModelDB>, ResponseBaseModel>()
            .RegisterMqListener<FormFieldDeleteConstructorReceive, TAuthRequestStandardModel<FormFieldDeleteRequestModel>, ResponseBaseModel>()
            .RegisterMqListener<FormFieldUpdateOrCreateConstructorReceive, TAuthRequestStandardModel<FieldFormConstructorModelDB>, ResponseBaseModel>()
            .RegisterMqListener<FieldDirectoryFormMoveConstructorReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<FormConstructorModelDB>>()
            .RegisterMqListener<FieldFormMoveConstructorReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<FormConstructorModelDB>>()
            .RegisterMqListener<FormDeleteConstructorReceive, TAuthRequestStandardModel<FormDeleteRequestModel>, ResponseBaseModel>()
            .RegisterMqListener<FormUpdateOrCreateConstructorReceive, TAuthRequestStandardModel<FormBaseConstructorModel>, TResponseModel<FormConstructorModelDB>>()
            .RegisterMqListener<GetFormConstructorReceive, int, TResponseModel<FormConstructorModelDB?>>()
            .RegisterMqListener<GetElementsOfDirectoryConstructorReceive, int, TResponseModel<List<EntryStandardModel>?>>()
            .RegisterMqListener<CreateElementForDirectoryConstructorReceive, TAuthRequestStandardModel<OwnedNameModel>, TResponseModel<int>>()
            .RegisterMqListener<UpdateElementOfDirectoryConstructorReceive, TAuthRequestStandardModel<EntryDescriptionModel>, ResponseBaseModel>()
            .RegisterMqListener<GetElementOfDirectoryConstructorReceive, int, TResponseModel<EntryDescriptionModel>>()
            .RegisterMqListener<DeleteElementFromDirectoryConstructorReceive, TAuthRequestStandardModel<DeleteElementFromDirectoryRequestModel>, ResponseBaseModel>()
            .RegisterMqListener<UpMoveElementOfDirectoryConstructorReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<DownMoveElementOfDirectoryConstructorReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<DeleteDirectoryConstructorReceive, TAuthRequestStandardModel<DeleteDirectoryRequestModel>, ResponseBaseModel>()
            .RegisterMqListener<UpdateOrCreateDirectoryConstructorReceive, TAuthRequestStandardModel<EntryConstructedModel>, TResponseModel<int>>()
            .RegisterMqListener<GetDirectoryConstructorReceive, int, TResponseModel<EntryDescriptionModel>>()
            .RegisterMqListener<GetDirectoriesConstructorReceive, ProjectFindModel, TResponseModel<EntryStandardModel[]>>()
            .RegisterMqListener<ReadDirectoriesConstructorReceive, int[], List<EntryNestedModel>>()
            .RegisterMqListener<GetCurrentMainProjectConstructorReceive, string, TResponseModel<MainProjectViewModel?>>()
            .RegisterMqListener<DeleteMembersFromProjectConstructorReceive, UsersProjectModel, ResponseBaseModel>()
            .RegisterMqListener<CanEditProjectConstructorReceive, UserProjectModel, ResponseBaseModel>()
            .RegisterMqListener<SetProjectAsMainConstructorReceive, UserProjectModel, ResponseBaseModel>()
            .RegisterMqListener<AddMembersToProjectConstructorReceive, UsersProjectModel, ResponseBaseModel>()
            .RegisterMqListener<GetMembersOfProjectConstructorReceive, int, TResponseModel<EntryAltStandardModel[]>>()
            .RegisterMqListener<UpdateProjectConstructorReceive, ProjectViewModel, ResponseBaseModel>()
            .RegisterMqListener<SetMarkerDeleteProjectConstructorReceive, SetMarkerProjectRequestModel, ResponseBaseModel>()
            .RegisterMqListener<GetProjectsForUserConstructorReceive, GetProjectsForUserRequestModel, TResponseModel<ProjectViewModel[]>>()
            ;
    }
}