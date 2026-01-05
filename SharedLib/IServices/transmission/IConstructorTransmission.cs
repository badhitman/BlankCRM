////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Constructor Remote Transmission Service
/// </summary>
public interface IConstructorTransmission
{
    #region public
    /// <summary>
    /// AddRowToTable
    /// </summary>
    public Task<TResponseModel<int>> AddRowToTableAsync(FieldSessionDocumentDataBaseModel req, CancellationToken token = default);

    /// <summary>
    /// DeleteValuesFieldsByGroupSessionDocumentDataByRowNum
    /// </summary>
    public Task<ResponseBaseModel> DeleteValuesFieldsByGroupSessionDocumentDataByRowNumAsync(ValueFieldSessionDocumentDataBaseModel req, CancellationToken token = default);

    /// <summary>
    /// SetDoneSessionDocumentData
    /// </summary>
    public Task<ResponseBaseModel> SetDoneSessionDocumentDataAsync(string req, CancellationToken token = default);

    /// <summary>
    /// SetValueFieldSessionDocumentData
    /// </summary>
    public Task<TResponseModel<SessionOfDocumentDataModelDB>> SetValueFieldSessionDocumentDataAsync(SetValueFieldDocumentDataModel req, CancellationToken token = default);

    /// <summary>
    /// GetSessionDocumentData
    /// </summary>
    public Task<TResponseModel<SessionOfDocumentDataModelDB>> GetSessionDocumentDataAsync(string req, CancellationToken token = default);
    #endregion

    #region derictories
    /// <summary>
    /// GetDirectory
    /// </summary>
    public Task<TResponseModel<EntryDescriptionModel>> GetDirectoryAsync(int req, CancellationToken token = default);

    /// <summary>
    /// GetDirectories
    /// </summary>
    public Task<TResponseModel<EntryModel[]>> GetDirectoriesAsync(ProjectFindModel req, CancellationToken token = default);

    /// <summary>
    /// ReadDirectories
    /// </summary>
    public Task<List<EntryNestedModel>> ReadDirectoriesAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// UpdateOrCreateDirectory
    /// </summary>
    public Task<TResponseModel<int>> UpdateOrCreateDirectoryAsync(TAuthRequestStandardModel<EntryConstructedModel> req, CancellationToken token = default);

    /// <summary>
    /// DeleteDirectory
    /// </summary>
    public Task<ResponseBaseModel> DeleteDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
    #endregion
    #region elements of directories
    /// <summary>
    /// GetElementsOfDirectory
    /// </summary>
    public Task<TResponseModel<List<EntryModel>>> GetElementsOfDirectoryAsync(int req, CancellationToken token = default);

    /// <summary>
    /// CreateElementForDirectory
    /// </summary>
    public Task<TResponseModel<int>> CreateElementForDirectoryAsync(TAuthRequestStandardModel<OwnedNameModel> req, CancellationToken token = default);

    /// <summary>
    /// UpdateElementOfDirectory
    /// </summary>
    public Task<ResponseBaseModel> UpdateElementOfDirectoryAsync(TAuthRequestStandardModel<EntryDescriptionModel> req, CancellationToken token = default);

    /// <summary>
    /// GetElementOfDirectory
    /// </summary>
    public Task<TResponseModel<EntryDescriptionModel>> GetElementOfDirectoryAsync(int req, CancellationToken token = default);

    /// <summary>
    /// DeleteElementFromDirectory
    /// </summary>
    public Task<ResponseBaseModel> DeleteElementFromDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// UpMoveElementOfDirectory
    /// </summary>
    public Task<ResponseBaseModel> UpMoveElementOfDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// DownMoveElementOfDirectory
    /// </summary>
    public Task<ResponseBaseModel> DownMoveElementOfDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// CheckAndNormalizeSortIndexForElementsOfDirectory
    /// </summary>
    public Task<ResponseBaseModel> CheckAndNormalizeSortIndexForElementsOfDirectoryAsync(int req, CancellationToken token = default);
    #endregion

    #region project
    /// <summary>
    /// CanEditProject
    /// </summary>
    public Task<ResponseBaseModel> CanEditProjectAsync(UserProjectModel req, CancellationToken token = default);

    /// <summary>
    /// DeleteMembersFromProject
    /// </summary>
    public Task<ResponseBaseModel> DeleteMembersFromProjectAsync(UsersProjectModel req, CancellationToken token = default);

    /// <summary>
    /// ProjectsRead
    /// </summary>
    public Task<List<ProjectModelDb>> ProjectsReadAsync(int[] ids, CancellationToken token = default);

    /// <summary>
    /// GetProjectsForUser
    /// </summary>
    public Task<TResponseModel<ProjectViewModel[]>> GetProjectsForUserAsync(GetProjectsForUserRequestModel req, CancellationToken token = default);

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
    /// SetProjectAsMain
    /// </summary>
    public Task<ResponseBaseModel> SetProjectAsMainAsync(UserProjectModel req, CancellationToken token = default);

    /// <summary>
    /// GetCurrentMainProject
    /// </summary>
    public Task<TResponseModel<MainProjectViewModel>> GetCurrentMainProjectAsync(string req, CancellationToken token = default);

    /// <summary>
    /// CreateProject
    /// </summary>
    public Task<TResponseModel<int>> CreateProjectAsync(CreateProjectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// GetMembersOfProject
    /// </summary>
    public Task<TResponseModel<EntryAltModel[]>> GetMembersOfProjectAsync(int req, CancellationToken token = default);
    #endregion

    /////////////// Формы для редактирования/добавления бизнес-сущностей внутри итогового документа.
    // Базовая бизнес-сущность описывающая каркас/строение данных. Можно сравнить с таблицей БД со своим набором полей/колонок
    // К тому же сразу настраивается web-форма для редактирования объекта данного типа. Возможность устанавливать css стили формам и полям (с умыслом использования возможностей Bootstrap)
    // Тип данных для полей форм может быть любой из перечня доступных: перечисление (созданное вами же), строка, число, булево, дата и т.д.
    #region формы
    /// <summary>
    /// Подобрать формы
    /// </summary>
    public Task<TPaginationResponseStandardModel<FormConstructorModelDB>> SelectFormsAsync(SelectFormsModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить форму
    /// </summary>
    public Task<TResponseModel<FormConstructorModelDB>> GetFormAsync(int form_id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить/создать форму (имя, описание, `признак таблицы`)
    /// </summary>
    public Task<TResponseModel<FormConstructorModelDB>> FormUpdateOrCreateAsync(TAuthRequestStandardModel<FormBaseConstructorModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить форму
    /// </summary>
    public Task<ResponseBaseModel> FormDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default);
    #endregion
    #region поля форм    
    /// <summary>
    /// Сдвинуть поле формы (простой тип)
    /// </summary>
    public Task<TResponseModel<FormConstructorModelDB>> FieldFormMoveAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сдвинуть поле формы (тип: список/справочник)
    /// </summary>
    public Task<TResponseModel<FormConstructorModelDB>> FieldDirectoryFormMoveAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default);


    /// <summary>
    /// Обновить/создать поле формы (простой тип)
    /// </summary>
    public Task<ResponseBaseModel> FormFieldUpdateOrCreateAsync(TAuthRequestStandardModel<FieldFormConstructorModelDB> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить поле формы (простой тип)
    /// </summary>
    public Task<ResponseBaseModel> FormFieldDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить/создать поле формы (тип: справочник/список)
    /// </summary>
    public Task<ResponseBaseModel> FormFieldDirectoryUpdateOrCreateAsync(TAuthRequestStandardModel<FieldFormAkaDirectoryConstructorModelDB> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить поле формы (тип: справочник/список)
    /// </summary>
    public Task<ResponseBaseModel> FormFieldDirectoryDeleteAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default);
    #endregion

    /////////////// Документ. Описывается/настраивается конечный результат, который будет использоваться.
    // Может содержать одну или несколько вкладок/табов. На каждом табе/вкладке может располагаться одна или больше форм
    // Каждая располагаемая форма может быть помечена как [Табличная]. Т.е. пользователь будет добавлять сколь угодно строк одной и той же формы.
    // Пользователь при добавлении/редактировании строк таблицы будет видеть форму, которую вы настроили для этого, а внутри таба это будет выглядеть как обычная многострочная таблица с колонками, равными полям формы
    #region документы
    /// <summary>
    /// Обновить/создать схему документа
    /// </summary>
    public Task<TResponseModel<DocumentSchemeConstructorModelDB>> UpdateOrCreateDocumentSchemeAsync(TAuthRequestStandardModel<EntryConstructedModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Запрос схем документов
    /// </summary>
    public Task<TPaginationResponseStandardModel<DocumentSchemeConstructorModelDB>> RequestDocumentsSchemesAsync(RequestDocumentsSchemesModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить схему документа
    /// </summary>
    public Task<TResponseModel<DocumentSchemeConstructorModelDB>> GetDocumentSchemeAsync(int questionnaire_id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить схему документа
    /// </summary>
    public Task<ResponseBaseModel> DeleteDocumentSchemeAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default);
    #endregion
    // табы/вкладки схожи по смыслу табов/вкладок в Excel. Т.е. обычная группировка разных рабочих пространств со своим именем 
    #region табы документов
    /// <summary>
    /// Обновить/создать таб/вкладку схемы документа
    /// </summary>
    public Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> CreateOrUpdateTabOfDocumentSchemeAsync(TAuthRequestStandardModel<EntryDescriptionOwnedModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Перемещение страницы опроса/анкеты (сортировка страниц внутри опроса/анкеты)
    /// </summary>
    public Task<TResponseModel<DocumentSchemeConstructorModelDB>> MoveTabOfDocumentSchemeAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить страницу анкеты/опроса
    /// </summary>
    public Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> GetTabOfDocumentSchemeAsync(int questionnaire_page_id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить страницу опроса/анкеты
    /// </summary>
    public Task<ResponseBaseModel> DeleteTabOfDocumentSchemeAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default);
    #endregion
    #region структура/схема таба/вкладки: формы, порядок и настройки поведения    
    /// <summary>
    /// Получить связь [таба/вкладки схемы документа] с [формой]
    /// </summary>
    public Task<TResponseModel<FormToTabJoinConstructorModelDB>> GetTabDocumentSchemeJoinFormAsync(int questionnaire_page_join_form_id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить/создать связь [таба/вкладки схемы документа] с [формой]
    /// </summary>
    public Task<ResponseBaseModel> CreateOrUpdateTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<FormToTabJoinConstructorModelDB> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сдвинуть связь [таба/вкладки схемы документа] с [формой] (изменение сортировки/последовательности)
    /// </summary>
    public Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> MoveTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить связь [таба/вкладки схемы документа] с [формой] 
    /// </summary>
    public Task<ResponseBaseModel> DeleteTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default);
    #endregion

    /////////////// Пользовательский/публичный доступ к возможностям заполнения документа данными
    // Если у вас есть готовый к заполнению документ со всеми его табами и настройками, то вы можете создавать уникальные ссылки для заполнения данными
    // Каждая ссылка это всего лишь уникальный GUID к которому привязываются все данные, которые вводят конечные пользователи
    // Пользователи видят ваш документ, но сам документ данные не хранит. Хранение данных происходит в сессиях, которые вы сами выпускаете для любого вашего документа
    #region сессии опросов/анкет
    /// <summary>
    /// Сохранить данные формы документа из сессии
    /// </summary>
    public Task<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>> SaveSessionFormAsync(SaveConstructorSessionRequestModel req, CancellationToken token = default);

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
    public Task<TResponseModel<EntryDictModel[]>> FindSessionsDocumentsByFormFieldNameAsync(FormFieldModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить значения (введённые в сессиях) по имени поля
    /// </summary>
    public Task<ResponseBaseModel> ClearValuesForFieldNameAsync(FormFieldOfSessionModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить сессию опроса/анкеты
    /// </summary>
    public Task<ResponseBaseModel> DeleteSessionDocumentAsync(int session_id, CancellationToken cancellationToken = default);
    #endregion     
}