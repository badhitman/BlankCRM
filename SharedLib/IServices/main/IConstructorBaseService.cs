////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IConstructorBaseService
/// </summary>
public interface IConstructorBaseService
{
    /////////////// Контекст работы конструктора: работы в системе над какими-либо сущностями всегда принадлежат какому-либо проекту/контексту.
    // При переключении контекста (текущий/основной проект) становятся доступны только работы по этому проекту
    // В проект можно добавлять участников, что бы те могли работать вместе с владельцем => вносить изменения в конструкторе данного проекта/контекста
    // Если проект отключить (есть у него такой статус: IsDisabled), то работы с проектом блокируются для всех участников, кроме владельца
    #region проекты
    /// <summary>
    /// GetProjectsForUser
    /// </summary>
    public Task<TResponseModel<ProjectViewModel[]>> GetProjectsForUserAsync(GetProjectsForUserRequestModel req, CancellationToken token = default);

    /// <summary>
    /// ProjectsRead
    /// </summary>
    public Task<List<ProjectModelDb>> ProjectsReadAsync(int[] ids, CancellationToken token = default);

    /// <summary>
    /// CreateProject
    /// </summary>
    public Task<TResponseModel<int>> CreateProjectAsync(CreateProjectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// SetMarkerDeleteProject
    /// </summary>
    public Task<ResponseBaseModel> SetMarkerDeleteProjectAsync(SetMarkerProjectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// UpdateProject
    /// </summary>
    public Task<ResponseBaseModel> UpdateProjectAsync(ProjectViewModel req, CancellationToken token = default);

    /// <summary>
    /// AddMembersToProject
    /// </summary>
    public Task<ResponseBaseModel> AddMembersToProjectAsync(UsersProjectModel req, CancellationToken token = default);

    /// <summary>
    /// DeleteMembersFromProject
    /// </summary>
    public Task<ResponseBaseModel> DeleteMembersFromProjectAsync(UsersProjectModel req, CancellationToken token = default);

    /// <summary>
    /// GetMembersOfProject
    /// </summary>
    public Task<TResponseModel<EntryAltStandardModel[]>> GetMembersOfProjectAsync(int req, CancellationToken token = default);

    /// <summary>
    /// SetProjectAsMain
    /// </summary>
    public Task<ResponseBaseModel> SetProjectAsMainAsync(UserProjectModel req, CancellationToken token = default);

    /// <summary>
    /// GetCurrentMainProject
    /// </summary>
    public Task<TResponseModel<MainProjectViewModel>> GetCurrentMainProjectAsync(GetCurrentMainProjectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Проверка пользователя на возможность проводить работы в контексте проекта.
    /// </summary>
    /// <remarks>
    /// Пользователи с ролью ADMIN имеют полный доступ ко всем проектам.
    /// Владельцы имеют полный доступ к своим проектам, а простые участники проектов зависят от статуса проекта (выкл/вкл)
    /// </remarks>
    public Task<ResponseBaseModel> CanEditProjectAsync(UserProjectModel req, CancellationToken token);
    #endregion

    /// <summary>
    /// Удалить страницу опроса/анкеты
    /// </summary>
    public Task<ResponseBaseModel> DeleteTabOfDocumentSchemeAsync(TAuthRequestStandardModel<DeleteTabOfDocumentSchemeRequestModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить связь [таба/вкладки схемы документа] с [формой] 
    /// </summary>
    public Task<ResponseBaseModel> DeleteTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<DeleteTabDocumentSchemeJoinFormRequestModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить поле формы (тип: справочник/список)
    /// </summary>
    public Task<ResponseBaseModel> FormFieldDirectoryDeleteAsync(TAuthRequestStandardModel<FormFieldDirectoryDeleteRequestModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить поле формы (простой тип)
    /// </summary>
    public Task<ResponseBaseModel> FormFieldDeleteAsync(TAuthRequestStandardModel<FormFieldDeleteRequestModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить схему документа
    /// </summary>
    public Task<ResponseBaseModel> DeleteDocumentSchemeAsync(TAuthRequestStandardModel<DeleteDocumentSchemeRequestModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить элемент справочника/списка
    /// </summary>
    public Task<ResponseBaseModel> DeleteElementFromDirectoryAsync(TAuthRequestStandardModel<DeleteElementFromDirectoryRequestModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить справочник/список (со всеми элементами и связями)
    /// </summary>
    public Task<ResponseBaseModel> DeleteDirectoryAsync(TAuthRequestStandardModel<DeleteDirectoryRequestModel> req, CancellationToken cancellationToken = default);
}
