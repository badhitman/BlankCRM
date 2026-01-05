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
            .RegisterMqListener<CreateProjectReceive, CreateProjectRequestModel, TResponseModel<int>>()
            .RegisterMqListener<ProjectsReadReceive, int[], List<ProjectModelDb>>()
            .RegisterMqListener<CheckAndNormalizeSortIndexForElementsOfDirectoryReceive, int, ResponseBaseModel>()
            .RegisterMqListener<AddRowToTableReceive, FieldSessionDocumentDataBaseModel, TResponseModel<int>>()
            .RegisterMqListener<DeleteValuesFieldsByGroupSessionDocumentDataByRowNumReceive, ValueFieldSessionDocumentDataBaseModel, ResponseBaseModel>()
            .RegisterMqListener<SetDoneSessionDocumentDataReceive, string, ResponseBaseModel>()
            .RegisterMqListener<SetValueFieldSessionDocumentDataReceive, SetValueFieldDocumentDataModel, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterMqListener<GetSessionDocumentDataReceive, string, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterMqListener<SelectFormsReceive, SelectFormsModel, TPaginationResponseStandardModel<FormConstructorModelDB>>()
            .RegisterMqListener<DeleteSessionDocumentReceive, int, ResponseBaseModel>()
            .RegisterMqListener<ClearValuesForFieldNameReceive, FormFieldOfSessionModel, ResponseBaseModel>()
            .RegisterMqListener<FindSessionsDocumentsByFormFieldNameReceive, FormFieldModel, TResponseModel<EntryDictModel[]>>()
            .RegisterMqListener<RequestSessionsDocumentsReceive, RequestSessionsDocumentsRequestPaginationModel, TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>>()
            .RegisterMqListener<UpdateOrCreateSessionDocumentReceive, SessionOfDocumentDataModelDB, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterMqListener<GetSessionDocumentReceive, SessionGetModel, TResponseModel<SessionOfDocumentDataModelDB?>>()
            .RegisterMqListener<SetStatusSessionDocumentReceive, SessionStatusModel, ResponseBaseModel>()
            .RegisterMqListener<SaveSessionFormReceive, SaveConstructorSessionRequestModel, TResponseModel<ValueDataForSessionOfDocumentModelDB[]>>()
            .RegisterMqListener<DeleteTabDocumentSchemeJoinFormReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<MoveTabDocumentSchemeJoinFormReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<TabOfDocumentSchemeConstructorModelDB>>()
            .RegisterMqListener<CreateOrUpdateTabDocumentSchemeJoinFormReceive, TAuthRequestStandardModel<FormToTabJoinConstructorModelDB>, ResponseBaseModel>()
            .RegisterMqListener<GetTabDocumentSchemeJoinFormReceive, int, TResponseModel<FormToTabJoinConstructorModelDB?>>()
            .RegisterMqListener<DeleteTabOfDocumentSchemeReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<GetTabOfDocumentSchemeReceive, int, TResponseModel<TabOfDocumentSchemeConstructorModelDB?>>()
            .RegisterMqListener<MoveTabOfDocumentSchemeReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<DocumentSchemeConstructorModelDB>>()
            .RegisterMqListener<CreateOrUpdateTabOfDocumentSchemeReceive, TAuthRequestStandardModel<EntryDescriptionOwnedModel>, TResponseModel<TabOfDocumentSchemeConstructorModelDB>>()
            .RegisterMqListener<DeleteDocumentSchemeReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<GetDocumentSchemeReceive, int, TResponseModel<DocumentSchemeConstructorModelDB?>>()
            .RegisterMqListener<RequestDocumentsSchemesReceive, RequestDocumentsSchemesModel, TPaginationResponseStandardModel<DocumentSchemeConstructorModelDB>>()
            .RegisterMqListener<UpdateOrCreateDocumentSchemeReceive, TAuthRequestStandardModel<EntryConstructedModel>, TResponseModel<DocumentSchemeConstructorModelDB?>>()
            .RegisterMqListener<FormFieldDirectoryDeleteReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<FormFieldDirectoryUpdateOrCreateReceive, TAuthRequestStandardModel<FieldFormAkaDirectoryConstructorModelDB>, ResponseBaseModel>()
            .RegisterMqListener<FormFieldDeleteReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<FormFieldUpdateOrCreateReceive, TAuthRequestStandardModel<FieldFormConstructorModelDB>, ResponseBaseModel>()
            .RegisterMqListener<FieldDirectoryFormMoveReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<FormConstructorModelDB>>()
            .RegisterMqListener<FieldFormMoveReceive, TAuthRequestStandardModel<MoveObjectModel>, TResponseModel<FormConstructorModelDB>>()
            .RegisterMqListener<FormDeleteReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<FormUpdateOrCreateReceive, TAuthRequestStandardModel<FormBaseConstructorModel>, TResponseModel<FormConstructorModelDB>>()
            .RegisterMqListener<GetFormReceive, int, TResponseModel<FormConstructorModelDB?>>()
            .RegisterMqListener<GetElementsOfDirectoryReceive, int, TResponseModel<List<EntryModel>?>>()
            .RegisterMqListener<CreateElementForDirectoryReceive, TAuthRequestStandardModel<OwnedNameModel>, TResponseModel<int>>()
            .RegisterMqListener<UpdateElementOfDirectoryReceive, TAuthRequestStandardModel<EntryDescriptionModel>, ResponseBaseModel>()
            .RegisterMqListener<GetElementOfDirectoryReceive, int, TResponseModel<EntryDescriptionModel>>()
            .RegisterMqListener<DeleteElementFromDirectoryReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<UpMoveElementOfDirectoryReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<DownMoveElementOfDirectoryReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<DeleteDirectoryReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<UpdateOrCreateDirectoryReceive, TAuthRequestStandardModel<EntryConstructedModel>, TResponseModel<int>>()
            .RegisterMqListener<GetDirectoryReceive, int, TResponseModel<EntryDescriptionModel>>()
            .RegisterMqListener<GetDirectoriesReceive, ProjectFindModel, TResponseModel<EntryModel[]>>()
            .RegisterMqListener<ReadDirectoriesReceive, int[], List<EntryNestedModel>>()
            .RegisterMqListener<GetCurrentMainProjectReceive, string, TResponseModel<MainProjectViewModel?>>()
            .RegisterMqListener<DeleteMembersFromProjectReceive, UsersProjectModel, ResponseBaseModel>()
            .RegisterMqListener<CanEditProjectReceive, UserProjectModel, ResponseBaseModel>()
            .RegisterMqListener<SetProjectAsMainReceive, UserProjectModel, ResponseBaseModel>()
            .RegisterMqListener<AddMembersToProjectReceive, UsersProjectModel, ResponseBaseModel>()
            .RegisterMqListener<GetMembersOfProjectReceive, int, TResponseModel<EntryAltModel[]>>()
            .RegisterMqListener<UpdateProjectReceive, ProjectViewModel, ResponseBaseModel>()
            .RegisterMqListener<SetMarkerDeleteProjectReceive, SetMarkerProjectRequestModel, ResponseBaseModel>()
            .RegisterMqListener<GetProjectsForUserReceive, GetProjectsForUserRequestModel, TResponseModel<ProjectViewModel[]>>()
            ;
    }
}