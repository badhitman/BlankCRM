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
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.AddRowToTableReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteValuesFieldsByGroupSessionDocumentDataByRowNumAsync(ValueFieldSessionDocumentDataBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteValuesFieldsByGroupSessionDocumentDataByRowNumReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetDoneSessionDocumentDataAsync(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetDoneSessionDocumentDataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> SetValueFieldSessionDocumentDataAsync(SetValueFieldDocumentDataModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.SetValueFieldSessionDocumentDataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocumentDataAsync(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetSessionDocumentDataReceive, req, token: token) ?? new();
    #endregion

    #region projects
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> AddMembersToProjectAsync(UsersProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.AddMembersToProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ProjectViewModel[]>> GetProjectsForUserAsync(GetProjectsForUserRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ProjectViewModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.ProjectsForUserReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<ProjectModelDb>> ProjectsReadAsync(int[] ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<ProjectModelDb>>(GlobalStaticConstantsTransmission.TransmissionQueues.ProjectsReadReceive, ids, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetMarkerDeleteProjectAsync(SetMarkerProjectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetMarkerDeleteProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateProjectAsync(ProjectViewModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetProjectAsMainAsync(UserProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetProjectAsMainReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MainProjectViewModel>> GetCurrentMainProjectAsync(string req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<MainProjectViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetCurrentMainProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateProjectAsync(CreateProjectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.ProjectCreateReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryAltModel[]>> GetMembersOfProjectAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryAltModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetMembersOfProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteMembersFromProjectAsync(UsersProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteMembersFromProjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CanEditProjectAsync(UserProjectModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CanEditProjectReceive, req, token: token) ?? new();
    #endregion

    #region directories
    /// <inheritdoc/>
    public async Task<TResponseModel<EntryModel[]>> GetDirectoriesAsync(ProjectFindModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetDirectoriesReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDescriptionModel>> GetDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryDescriptionModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<EntryNestedModel>> ReadDirectoriesAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<EntryNestedModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadDirectoriesReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> UpdateOrCreateDirectoryAsync(TAuthRequestStandardModel<EntryConstructedModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDirectoryReceive, req, token: token) ?? new();
    #endregion
    #region elements of directories
    /// <inheritdoc/>
    public async Task<TResponseModel<List<EntryModel>>> GetElementsOfDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<EntryModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetElementsOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateElementForDirectoryAsync(TAuthRequestStandardModel<OwnedNameModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.CreateElementForDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateElementOfDirectoryAsync(TAuthRequestStandardModel<EntryDescriptionModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDescriptionModel>> GetElementOfDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryDescriptionModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteElementFromDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteElementFromDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpMoveElementOfDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpMoveElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownMoveElementOfDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DownMoveElementOfDirectoryReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CheckAndNormalizeSortIndexForElementsOfDirectoryAsync(int req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CheckAndNormalizeSortIndexForElementsOfDirectoryReceive, req, token: token) ?? new();
    #endregion

    #region forms
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<FormConstructorModelDB>> SelectFormsAsync(SelectFormsModel req, CancellationToken cancellationToken = default)
    => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<FormConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.SelectFormsReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> GetFormAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FormUpdateOrCreateAsync(TAuthRequestStandardModel<FormBaseConstructorModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.FormUpdateOrCreateReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FormDeleteReceive, req, token: cancellationToken) ?? new();
    #endregion
    #region fiealds
    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FieldFormMoveAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.FieldFormMoveReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormConstructorModelDB>> FieldDirectoryFormMoveAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.FieldDirectoryFormMoveReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldUpdateOrCreateAsync(TAuthRequestStandardModel<FieldFormConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FormFieldUpdateOrCreateReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FormFieldDeleteReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDirectoryUpdateOrCreateAsync(TAuthRequestStandardModel<FieldFormAkaDirectoryConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FormFieldDirectoryUpdateOrCreateReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FormFieldDirectoryDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FormFieldDirectoryDeleteReceive, req, token: cancellationToken) ?? new();
    #endregion

    #region documents
    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> UpdateOrCreateDocumentSchemeAsync(TAuthRequestStandardModel<EntryConstructedModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DocumentSchemeConstructorModelDB>> RequestDocumentsSchemesAsync(RequestDocumentsSchemesModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.RequestDocumentsSchemesReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> GetDocumentSchemeAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDocumentSchemeAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDocumentSchemeReceive, req, token: cancellationToken) ?? new();
    #endregion
    #region табы документов
    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> CreateOrUpdateTabOfDocumentSchemeAsync(TAuthRequestStandardModel<EntryDescriptionOwnedModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.CreateOrUpdateTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentSchemeConstructorModelDB>> MoveTabOfDocumentSchemeAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<DocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.MoveTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> GetTabOfDocumentSchemeAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteTabOfDocumentSchemeAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteTabOfDocumentSchemeReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FormToTabJoinConstructorModelDB>> GetTabDocumentSchemeJoinFormAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<FormToTabJoinConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateOrUpdateTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<FormToTabJoinConstructorModelDB> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CreateOrUpdateTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> MoveTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<TabOfDocumentSchemeConstructorModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.MoveTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteTabDocumentSchemeJoinFormReceive, req, token: cancellationToken) ?? new();
    #endregion

    #region session
    /// <inheritdoc/>
    public async Task<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>> SaveSessionFormAsync(SaveConstructorSessionRequestModel req, CancellationToken token = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveSessionFormReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetStatusSessionDocumentAsync(SessionStatusModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetStatusSessionDocumentReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocumentAsync(SessionGetModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetSessionDocumentReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB>> UpdateOrCreateSessionDocumentAsync(SessionOfDocumentDataModelDB req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateSessionDocumentReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>> RequestSessionsDocumentsAsync(RequestSessionsDocumentsRequestPaginationModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.RequestSessionsDocumentsReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<EntryDictModel[]>> FindSessionsDocumentsByFormFieldNameAsync(FormFieldModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<TResponseModel<EntryDictModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.FindSessionsDocumentsByFormFieldNameReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClearValuesForFieldNameAsync(FormFieldOfSessionModel req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ClearValuesForFieldNameReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteSessionDocumentAsync(int req, CancellationToken cancellationToken = default)
     => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteSessionDocumentReceive, req, token: cancellationToken) ?? new();
    #endregion
}