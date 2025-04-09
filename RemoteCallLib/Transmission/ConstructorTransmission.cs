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
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.AddRowToTableReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteValuesFieldsByGroupSessionDocumentDataByRowNumAsync(ValueFieldSessionDocumentDataBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteValuesFieldsByGroupSessionDocumentDataByRowNumReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetDoneSessionDocumentDataAsync(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetDoneSessionDocumentDataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> SetValueFieldSessionDocumentDataAsync(SetValueFieldDocumentDataModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstants.TransmissionQueues.SetValueFieldSessionDocumentDataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocumentDataAsync(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstants.TransmissionQueues.GetSessionDocumentDataReceive, req, token: token) ?? new();
    #endregion

    #region projects
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddMembersToProjectAsync(UsersProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.AddMembersToProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ProjectViewModel[]>> GetProjectsForUserAsync(GetProjectsForUserRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ProjectViewModel[]>>(GlobalStaticConstants.TransmissionQueues.ProjectsForUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<ProjectModelDb>> ProjectsReadAsync(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<ProjectModelDb>>(GlobalStaticConstants.TransmissionQueues.ProjectsReadReceive, ids, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetMarkerDeleteProjectAsync(SetMarkerProjectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetMarkerDeleteProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateProjectAsync(ProjectViewModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.UpdateProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetProjectAsMainAsync(UserProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetProjectAsMainReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MainProjectViewModel>> GetCurrentMainProjectAsync(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<MainProjectViewModel>>(GlobalStaticConstants.TransmissionQueues.GetCurrentMainProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateProjectAsync(CreateProjectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.ProjectCreateReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryAltModel[]>> GetMembersOfProjectAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryAltModel[]>>(GlobalStaticConstants.TransmissionQueues.GetMembersOfProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteMembersFromProjectAsync(UsersProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteMembersFromProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CanEditProjectAsync(UserProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.CanEditProjectReceive, req, token: token) ?? new();
    #endregion

    #region directories
    /// <inheritdoc/>
    public async Task<TResponseModel<EntryModel[]>> GetDirectoriesAsync(ProjectFindModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryModel[]>>(GlobalStaticConstants.TransmissionQueues.GetDirectoriesReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDescriptionModel>> GetDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryDescriptionModel>>(GlobalStaticConstants.TransmissionQueues.GetDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<EntryNestedModel>> ReadDirectoriesAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<EntryNestedModel>>(GlobalStaticConstants.TransmissionQueues.ReadDirectoriesReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> UpdateOrCreateDirectoryAsync(TAuthRequestModel<EntryConstructedModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.UpdateOrCreateDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDirectoryAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteDirectoryReceive, req, token: token) ?? new();
    #endregion
    #region elements of directories
    /// <inheritdoc/>
    public async Task<TResponseModel<List<EntryModel>>> GetElementsOfDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<EntryModel>>>(GlobalStaticConstants.TransmissionQueues.GetElementsOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateElementForDirectoryAsync(TAuthRequestModel<OwnedNameModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.CreateElementForDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateElementOfDirectoryAsync(TAuthRequestModel<EntryDescriptionModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.UpdateElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDescriptionModel>> GetElementOfDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryDescriptionModel>>(GlobalStaticConstants.TransmissionQueues.GetElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteElementFromDirectoryAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteElementFromDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpMoveElementOfDirectoryAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.UpMoveElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownMoveElementOfDirectoryAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DownMoveElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CheckAndNormalizeSortIndexForElementsOfDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.CheckAndNormalizeSortIndexForElementsOfDirectoryReceive, req, token: token) ?? new();
    #endregion

    #region forms
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<FormConstructorModelDB>> SelectFormsAsync(SelectFormsModel req, CancellationToken cancellationToken = default)
    => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<FormConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.SelectFormsReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> GetFormAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.GetFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FormUpdateOrCreateAsync(TAuthRequestModel<FormBaseConstructorModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.FormUpdateOrCreateReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormDeleteAsync(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FormDeleteReceive, req, token: cancellationToken) ?? new();
    #endregion
    #region fiealds
    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FieldFormMoveAsync(TAuthRequestModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.FieldFormMoveReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FieldDirectoryFormMoveAsync(TAuthRequestModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.FieldDirectoryFormMoveReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldUpdateOrCreateAsync(TAuthRequestModel<FieldFormConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FormFieldUpdateOrCreateReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDeleteAsync(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FormFieldDeleteReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDirectoryUpdateOrCreateAsync(TAuthRequestModel<FieldFormAkaDirectoryConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FormFieldDirectoryUpdateOrCreateReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDirectoryDeleteAsync(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FormFieldDirectoryDeleteReceive, req, token: cancellationToken) ?? new();
    #endregion

    #region documents
    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> UpdateOrCreateDocumentSchemeAsync(TAuthRequestModel<EntryConstructedModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.UpdateOrCreateDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DocumentSchemeConstructorModelDB>> RequestDocumentsSchemesAsync(RequestDocumentsSchemesModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.RequestDocumentsSchemesReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> GetDocumentSchemeAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.GetDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDocumentSchemeAsync(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteDocumentSchemeReceive, req, token: cancellationToken) ?? new();
    #endregion
    #region табы документов
    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> CreateOrUpdateTabOfDocumentSchemeAsync(TAuthRequestModel<EntryDescriptionOwnedModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.CreateOrUpdateTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> MoveTabOfDocumentSchemeAsync(TAuthRequestModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.MoveTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> GetTabOfDocumentSchemeAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.GetTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteTabOfDocumentSchemeAsync(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormToTabJoinConstructorModelDB>> GetTabDocumentSchemeJoinFormAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormToTabJoinConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.GetTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateOrUpdateTabDocumentSchemeJoinFormAsync(TAuthRequestModel<FormToTabJoinConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.CreateOrUpdateTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> MoveTabDocumentSchemeJoinFormAsync(TAuthRequestModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstants.TransmissionQueues.MoveTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteTabDocumentSchemeJoinFormAsync(TAuthRequestModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();
    #endregion

    #region session
    /// <inheritdoc/>
    public async Task<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>> SaveSessionFormAsync(SaveConstructorSessionRequestModel req, CancellationToken token = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>>(GlobalStaticConstants.TransmissionQueues.SaveSessionFormReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetStatusSessionDocumentAsync(SessionStatusModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetStatusSessionDocumentReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocumentAsync(SessionGetModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstants.TransmissionQueues.GetSessionDocumentReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> UpdateOrCreateSessionDocumentAsync(SessionOfDocumentDataModelDB req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstants.TransmissionQueues.UpdateOrCreateSessionDocumentReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<SessionOfDocumentDataModelDB>> RequestSessionsDocumentsAsync(RequestSessionsDocumentsRequestPaginationModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstants.TransmissionQueues.RequestSessionsDocumentsReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDictModel[]>> FindSessionsDocumentsByFormFieldNameAsync(FormFieldModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryDictModel[]>>(GlobalStaticConstants.TransmissionQueues.FindSessionsDocumentsByFormFieldNameReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClearValuesForFieldNameAsync(FormFieldOfSessionModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.ClearValuesForFieldNameReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteSessionDocumentAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.DeleteSessionDocumentReceive, req, token: cancellationToken) ?? new();
    #endregion
}