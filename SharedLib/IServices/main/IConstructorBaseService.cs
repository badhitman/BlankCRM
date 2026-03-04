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

    #region поля форм
    /// <summary>
    /// Обновить/создать поле формы (тип: справочник/список)
    /// </summary>
    public Task<ResponseBaseModel> FormFieldDirectoryUpdateOrCreateAsync(TAuthRequestStandardModel<FieldFormAkaDirectoryConstructorModelDB> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить/создать поле формы (простой тип)
    /// </summary>
    public Task<ResponseBaseModel> FormFieldUpdateOrCreateAsync(TAuthRequestStandardModel<FieldFormConstructorModelDB> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сдвинуть поле формы (тип: список/справочник)
    /// </summary>
    public Task<TResponseModel<FormConstructorModelDB>> FieldDirectoryFormMoveAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сдвинуть поле формы (простой тип)
    /// </summary>                                                                                                                                                                        
    public Task<TResponseModel<FormConstructorModelDB>> FieldFormMoveAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default);
    #endregion

    /////////////// Пользовательский/публичный доступ к возможностям заполнения документа данными
    // Если у вас есть готовый к заполнению документ со всеми его табами и настройками, то вы можете создавать уникальные ссылки для заполнения данными
    // Каждая ссылка это всего лишь уникальный GUID к которому привязываются все данные, которые вводят конечные пользователи
    // Пользователи видят ваш документ, но сам документ данные не хранит. Хранение данных происходит в сессиях, которые вы сами выпускаете для любого вашего документа
    #region сессии опросов/анкет
    /// <summary>
    /// Сохранить данные формы документа из сессии
    /// </summary>
    public Task<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>> SaveSessionFormAsync(SaveConstructorSessionRequestModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Установить статус сессии (от менеджера)
    /// </summary>
    public Task<ResponseBaseModel> SetStatusSessionDocumentAsync(SessionStatusModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить сессию
    /// </summary>
    public Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocumentAsync(SessionGetModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить (или создать) сессию опроса/анкеты
    /// </summary>
    public Task<TResponseModel<SessionOfDocumentDataModelDB>> UpdateOrCreateSessionDocumentAsync(SessionOfDocumentDataModelDB session, CancellationToken cancellationToken = default);

    /// <summary>
    /// Запросить порцию сессий (с пагинацией)
    /// </summary>
    public Task<TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>> RequestSessionsDocumentsAsync(RequestSessionsDocumentsRequestPaginationModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Найти порцию сессий по имени поля (с пагинацией)
    /// </summary>
    public Task<TResponseModel<EntryDictStandardModel[]>> FindSessionsDocumentsByFormFieldNameAsync(FormFieldModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить значения (введённые в сессиях) по имени поля
    /// </summary>
    public Task<ResponseBaseModel> ClearValuesForFieldNameAsync(FormFieldOfSessionModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить сессию опроса/анкеты
    /// </summary>
    public Task<ResponseBaseModel> DeleteSessionDocumentAsync(DeleteSessionDocumentRequestModel session_id, CancellationToken cancellationToken = default);
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
