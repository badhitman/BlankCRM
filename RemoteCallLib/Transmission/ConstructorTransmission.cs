////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// TransmissionConstructorService
/// </summary>
public class ConstructorTransmission(IRabbitClient rabbitClient) : IConstructorTransmission
{
    #region public
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> AddRowToTable(FieldSessionDocumentDataBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.AddRowToTableReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteValuesFieldsByGroupSessionDocumentDataByRowNum(ValueFieldSessionDocumentDataBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteValuesFieldsByGroupSessionDocumentDataByRowNumReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetDoneSessionDocumentData(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetDoneSessionDocumentDataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> SetValueFieldSessionDocumentData(SetValueFieldDocumentDataModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstants.TransmissionQueues.SetValueFieldSessionDocumentDataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocumentData(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstants.TransmissionQueues.GetSessionDocumentDataReceive, req, token: token) ?? new();
    #endregion

    #region projects
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddMembersToProject(UsersProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.AddMembersToProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ProjectViewModel[]>> GetProjectsForUser(GetProjectsForUserRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<ProjectViewModel[]>>(GlobalStaticConstants.TransmissionQueues.ProjectsForUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<ProjectModelDb>> ProjectsRead(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<List<ProjectModelDb>>(GlobalStaticConstants.TransmissionQueues.ProjectsReadReceive, ids, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetMarkerDeleteProject(SetMarkerProjectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetMarkerDeleteProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateProject(ProjectViewModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.UpdateProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetProjectAsMain(UserProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetProjectAsMainReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MainProjectViewModel>> GetCurrentMainProject(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<MainProjectViewModel>>(GlobalStaticConstants.TransmissionQueues.GetCurrentMainProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateProject(CreateProjectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.ProjectCreateReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryAltModel[]>> GetMembersOfProject(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<EntryAltModel[]>>(GlobalStaticConstants.TransmissionQueues.GetMembersOfProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteMembersFromProject(UsersProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteMembersFromProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CanEditProject(UserProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.CanEditProjectReceive, req, token: token) ?? new();
    #endregion

    #region directories
    /// <inheritdoc/>
    public async Task<TResponseModel<EntryModel[]>> GetDirectories(ProjectFindModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<EntryModel[]>>(GlobalStaticConstants.TransmissionQueues.GetDirectoriesReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDescriptionModel>> GetDirectory(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<EntryDescriptionModel>>(GlobalStaticConstants.TransmissionQueues.GetDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<EntryNestedModel>> ReadDirectories(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<List<EntryNestedModel>>(GlobalStaticConstants.TransmissionQueues.ReadDirectoriesReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> UpdateOrCreateDirectory(TAuthRequestModel<EntryConstructedModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.UpdateOrCreateDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDirectory(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteDirectoryReceive, req, token: token) ?? new();
    #endregion
    #region elements of directories
    /// <inheritdoc/>
    public async Task<TResponseModel<List<EntryModel>>> GetElementsOfDirectory(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<List<EntryModel>>>(GlobalStaticConstants.TransmissionQueues.GetElementsOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateElementForDirectory(TAuthRequestModel<OwnedNameModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.CreateElementForDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateElementOfDirectory(TAuthRequestModel<EntryDescriptionModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.UpdateElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDescriptionModel>> GetElementOfDirectory(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<EntryDescriptionModel>>(GlobalStaticConstants.TransmissionQueues.GetElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteElementFromDirectory(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteElementFromDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpMoveElementOfDirectory(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.UpMoveElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownMoveElementOfDirectory(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DownMoveElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CheckAndNormalizeSortIndexForElementsOfDirectory(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.CheckAndNormalizeSortIndexForElementsOfDirectoryReceive, req, token: token) ?? new();
    #endregion

    #region forms
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<FormConstructorModelDB>> SelectForms(SelectFormsModel req, CancellationToken cancellationToken = default)
    => await rabbitClient.MqRemoteCall<TPaginationResponseModel<FormConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.SelectFormsReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> GetForm(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.GetFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FormUpdateOrCreate(TAuthRequestModel<FormBaseConstructorModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.FormUpdateOrCreateReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormDelete(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FormDeleteReceive, req, token: cancellationToken) ?? new();
    #endregion
    #region fiealds
    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FieldFormMove(TAuthRequestModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.FieldFormMoveReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FieldDirectoryFormMove(TAuthRequestModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.FieldDirectoryFormMoveReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldUpdateOrCreate(TAuthRequestModel<FieldFormConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FormFieldUpdateOrCreateReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDelete(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FormFieldDeleteReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDirectoryUpdateOrCreate(TAuthRequestModel<FieldFormAkaDirectoryConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FormFieldDirectoryUpdateOrCreateReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDirectoryDelete(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FormFieldDirectoryDeleteReceive, req, token: cancellationToken) ?? new();
    #endregion

    #region documents
    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> UpdateOrCreateDocumentScheme(TAuthRequestModel<EntryConstructedModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.UpdateOrCreateDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DocumentSchemeConstructorModelDB>> RequestDocumentsSchemes(RequestDocumentsSchemesModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TPaginationResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.RequestDocumentsSchemesReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> GetDocumentScheme(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.GetDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDocumentScheme(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteDocumentSchemeReceive, req, token: cancellationToken) ?? new();
    #endregion
    #region табы документов
    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> CreateOrUpdateTabOfDocumentScheme(TAuthRequestModel<EntryDescriptionOwnedModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.CreateOrUpdateTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> MoveTabOfDocumentScheme(TAuthRequestModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.MoveTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> GetTabOfDocumentScheme(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.GetTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteTabOfDocumentScheme(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormToTabJoinConstructorModelDB>> GetTabDocumentSchemeJoinForm(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<FormToTabJoinConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.GetTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateOrUpdateTabDocumentSchemeJoinForm(TAuthRequestModel<FormToTabJoinConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.CreateOrUpdateTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> MoveTabDocumentSchemeJoinForm(TAuthRequestModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.MoveTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteTabDocumentSchemeJoinForm(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();
    #endregion

    #region session
    /// <inheritdoc/>
    public async Task<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>> SaveSessionForm(SaveConstructorSessionRequestModel req, CancellationToken token = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>>(GlobalStaticConstants.TransmissionQueues.SaveSessionFormReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetStatusSessionDocument(SessionStatusModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetStatusSessionDocumentReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocument(SessionGetModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstants.TransmissionQueues.GetSessionDocumentReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> UpdateOrCreateSessionDocument(SessionOfDocumentDataModelDB req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstants.TransmissionQueues.UpdateOrCreateSessionDocumentReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<SessionOfDocumentDataModelDB>> RequestSessionsDocuments(RequestSessionsDocumentsRequestPaginationModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TPaginationResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstants.TransmissionQueues.RequestSessionsDocumentsReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDictModel[]>> FindSessionsDocumentsByFormFieldName(FormFieldModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<TResponseModel<EntryDictModel[]>>(GlobalStaticConstants.TransmissionQueues.FindSessionsDocumentsByFormFieldNameReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClearValuesForFieldName(FormFieldOfSessionModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.ClearValuesForFieldNameReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteSessionDocument(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteSessionDocumentReceive, req, token: cancellationToken) ?? new();
    #endregion
}