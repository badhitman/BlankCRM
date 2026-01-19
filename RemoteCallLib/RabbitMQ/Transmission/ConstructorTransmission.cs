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
    public async Task<TResponseModel<int>> AddRowToTableAsync(FieldSessionDocumentDataBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.AddRowToTableConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteValuesFieldsByGroupSessionDocumentDataByRowNumAsync(ValueFieldSessionDocumentDataBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteValuesFieldsByGroupSessionDocumentDataByRowNumConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetDoneSessionDocumentDataAsync(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetDoneSessionDocumentDataConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> SetValueFieldSessionDocumentDataAsync(SetValueFieldDocumentDataModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.SetValueFieldSessionDocumentDataConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocumentDataAsync(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetSessionDocumentDataConstructorReceive, req, token: token) ?? new();
    #endregion

    #region projects
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddMembersToProjectAsync(UsersProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.AddMembersToProjectConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ProjectViewModel[]>> GetProjectsForUserAsync(GetProjectsForUserRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ProjectViewModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.ProjectsForUserConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<ProjectModelDb>> ProjectsReadAsync(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<ProjectModelDb>>(GlobalStaticConstantsTransmission.TransmissionQueues.ProjectsReadConstructorReceive, ids, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetMarkerDeleteProjectAsync(SetMarkerProjectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetMarkerDeleteProjectConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateProjectAsync(ProjectViewModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateProjectConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetProjectAsMainAsync(UserProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetProjectAsMainConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MainProjectViewModel>> GetCurrentMainProjectAsync(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<MainProjectViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetCurrentMainProjectConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateProjectAsync(CreateProjectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.ProjectCreateConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryAltModel[]>> GetMembersOfProjectAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryAltModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetMembersOfProjectConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteMembersFromProjectAsync(UsersProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteMembersFromProjectConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CanEditProjectAsync(UserProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CanEditProjectConstructorReceive, req, token: token) ?? new();
    #endregion

    #region directories
    /// <inheritdoc/>
    public async Task<TResponseModel<EntryModel[]>> GetDirectoriesAsync(ProjectFindModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetDirectoriesConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDescriptionModel>> GetDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryDescriptionModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetDirectoryConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<EntryNestedModel>> ReadDirectoriesAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<EntryNestedModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadDirectoriesConstructorReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> UpdateOrCreateDirectoryAsync(TAuthRequestStandardModel<EntryConstructedModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateDirectoryConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDirectoryAsync(TAuthRequestStandardModel<DeleteDirectoryRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDirectoryConstructorReceive, req, token: token) ?? new();
    #endregion
    #region elements of directories
    /// <inheritdoc/>
    public async Task<TResponseModel<List<EntryModel>>> GetElementsOfDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<EntryModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetElementsOfDirectoryConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateElementForDirectoryAsync(TAuthRequestStandardModel<OwnedNameModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.CreateElementForDirectoryConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateElementOfDirectoryAsync(TAuthRequestStandardModel<EntryDescriptionModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateElementOfDirectoryConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDescriptionModel>> GetElementOfDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryDescriptionModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetElementOfDirectoryConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteElementFromDirectoryAsync(TAuthRequestStandardModel<DeleteElementFromDirectoryRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteElementFromDirectoryConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpMoveElementOfDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpMoveElementOfDirectoryConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownMoveElementOfDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DownMoveElementOfDirectoryConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CheckAndNormalizeSortIndexForElementsOfDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CheckAndNormalizeSortIndexForElementsOfDirectoryConstructorReceive, req, token: token) ?? new();
    #endregion

    #region forms
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<FormConstructorModelDB>> SelectFormsAsync(SelectFormsModel req, CancellationToken cancellationToken = default)
    => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<FormConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.SelectFormsConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> GetFormAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetFormConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FormUpdateOrCreateAsync(TAuthRequestStandardModel<FormBaseConstructorModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.FormUpdateOrCreateConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormDeleteAsync(TAuthRequestStandardModel<FormDeleteRequestModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FormDeleteConstructorReceive, req, token: cancellationToken) ?? new();
    #endregion
    #region fiealds
    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FieldFormMoveAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.FieldFormMoveConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FieldDirectoryFormMoveAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.FieldDirectoryFormMoveConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldUpdateOrCreateAsync(TAuthRequestStandardModel<FieldFormConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FormFieldUpdateOrCreateConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDeleteAsync(TAuthRequestStandardModel<FormFieldDeleteRequestModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FormFieldDeleteConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDirectoryUpdateOrCreateAsync(TAuthRequestStandardModel<FieldFormAkaDirectoryConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FormFieldDirectoryUpdateOrCreateConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDirectoryDeleteAsync(TAuthRequestStandardModel<FormFieldDirectoryDeleteRequestModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FormFieldDirectoryDeleteConstructorReceive, req, token: cancellationToken) ?? new();
    #endregion

    #region documents
    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> UpdateOrCreateDocumentSchemeAsync(TAuthRequestStandardModel<EntryConstructedModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateDocumentSchemeConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DocumentSchemeConstructorModelDB>> RequestDocumentsSchemesAsync(RequestDocumentsSchemesModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.RequestDocumentsSchemesConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> GetDocumentSchemeAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetDocumentSchemeConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDocumentSchemeAsync(TAuthRequestStandardModel<DeleteDocumentSchemeRequestModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDocumentSchemeConstructorReceive, req, token: cancellationToken) ?? new();
    #endregion
    #region табы документов
    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> CreateOrUpdateTabOfDocumentSchemeAsync(TAuthRequestStandardModel<EntryDescriptionOwnedModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.CreateOrUpdateTabOfDocumentSchemeConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> MoveTabOfDocumentSchemeAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.MoveTabOfDocumentSchemeConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> GetTabOfDocumentSchemeAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetTabOfDocumentSchemeConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteTabOfDocumentSchemeAsync(TAuthRequestStandardModel<DeleteTabOfDocumentSchemeRequestModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteTabOfDocumentSchemeConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormToTabJoinConstructorModelDB>> GetTabDocumentSchemeJoinFormAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormToTabJoinConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetTabDocumentSchemeJoinFormConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateOrUpdateTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<FormToTabJoinConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CreateOrUpdateTabDocumentSchemeJoinFormConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> MoveTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.MoveTabDocumentSchemeJoinFormConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<DeleteTabDocumentSchemeJoinFormRequestModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteTabDocumentSchemeJoinFormConstructorReceive, req, token: cancellationToken) ?? new();
    #endregion

    #region session
    /// <inheritdoc/>
    public async Task<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>> SaveSessionFormAsync(SaveConstructorSessionRequestModel req, CancellationToken token = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveSessionFormConstructorReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetStatusSessionDocumentAsync(SessionStatusModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetStatusSessionDocumentConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocumentAsync(SessionGetModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetSessionDocumentConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> UpdateOrCreateSessionDocumentAsync(SessionOfDocumentDataModelDB req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateSessionDocumentConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>> RequestSessionsDocumentsAsync(RequestSessionsDocumentsRequestPaginationModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.RequestSessionsDocumentsConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDictModel[]>> FindSessionsDocumentsByFormFieldNameAsync(FormFieldModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryDictModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.FindSessionsDocumentsByFormFieldNameConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClearValuesForFieldNameAsync(FormFieldOfSessionModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ClearValuesForFieldNameConstructorReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteSessionDocumentAsync(DeleteSessionDocumentRequestModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteSessionDocumentConstructorReceive, req, token: cancellationToken) ?? new();
    #endregion
}