////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Constructor служба
/// </summary>
public interface IConstructorService : IConstructorBaseService
{
    #region public
    /// <summary>
    /// Получить сессию
    /// </summary>
    public Task<TResponseModel<SessionOfDocumentDataModelDB?>> GetSessionDocumentDataAsync(string guid_session, CancellationToken cancellationToken = default);

    /// <summary>
    /// Установить значение свойства сессии
    /// </summary>
    public Task<TResponseModel<SessionOfDocumentDataModelDB>> SetValueFieldSessionDocumentDataAsync(SetValueFieldDocumentDataModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Отправить опрос на проверку (от клиента)
    /// </summary>
    public Task<ResponseBaseModel> SetDoneSessionDocumentDataAsync(string token_session, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить набор значений сессии опроса/анкеты по номеру строки [GroupByRowNum].
    /// Если индекс ниже нуля - удаляются все значения для указанной JoinForm (полная очистка таблицы или очистка всех значений всех полей стандартной формы)
    /// </summary>
    public Task<ResponseBaseModel> DeleteValuesFieldsByGroupSessionDocumentDataByRowNumAsync(ValueFieldSessionDocumentDataBaseModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить новую строку в таблицу значений
    /// </summary>
    /// <returns>Номер п/п (начиная с 1) созданной строки</returns>
    public Task<TResponseModel<int>> AddRowToTableAsync(FieldSessionDocumentDataBaseModel req, CancellationToken cancellationToken = default);
    #endregion

    /////////////// Перечисления.
    // Простейший тип данных поля формы, который можно в в последствии использовать в конструкторе форм при добавлении/редактировании полей
    // Примечание: В генераторе для C# .NET формируются как Enum
    #region справочники/списки
    /// <summary>
    /// Прочитать данные справочников
    /// </summary>
    public Task<List<EntryNestedModel>> ReadDirectoriesAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить справочники/списки для проекта
    /// </summary>
    public Task<TResponseModel<EntryStandardModel[]>> GetDirectoriesAsync(ProjectFindModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить справочник (список/перечисление)
    /// </summary>
    public Task<TResponseModel<EntryDescriptionModel>> GetDirectoryAsync(int enumeration_id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить/Создать справочник
    /// </summary>
    public Task<TResponseModel<int>> UpdateOrCreateDirectoryAsync(TAuthRequestStandardModel<EntryConstructedModel> req, CancellationToken cancellationToken = default);
    #endregion
    #region элементы справочникв/списков
    /// <summary>
    /// Получить элементы справочника/списка
    /// </summary>
    public Task<TResponseModel<List<EntryStandardModel>?>> GetElementsOfDirectoryAsync(int directory_id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Создать элемент справочника
    /// </summary>
    public Task<TResponseModel<int>> CreateElementForDirectoryAsync(TAuthRequestStandardModel<OwnedNameModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить элемент справочника
    /// </summary>
    public Task<ResponseBaseModel> UpdateElementOfDirectoryAsync(TAuthRequestStandardModel<EntryDescriptionModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить элемент справочника/перечисления/списка
    /// </summary>
    public Task<TResponseModel<EntryDescriptionModel>> GetElementOfDirectoryAsync(int element_id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сдвинуть выше элемент справочника/списка
    /// </summary>
    public Task<ResponseBaseModel> UpMoveElementOfDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сдвинуть ниже элемент справочника/списка
    /// </summary>
    public Task<ResponseBaseModel> DownMoveElementOfDirectoryAsync(TAuthRequestStandardModel<int> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Нормализовать индексы сортировки элементов справочника.
    /// </summary>
    public Task<ResponseBaseModel> CheckAndNormalizeSortIndexForElementsOfDirectoryAsync(int directory_id, CancellationToken cancellationToken = default);
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
    public Task<TResponseModel<FormConstructorModelDB?>> GetFormAsync(int form_id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить/создать форму (имя, описание, `признак таблицы`)
    /// </summary>
    public Task<TResponseModel<FormConstructorModelDB>> FormUpdateOrCreateAsync(TAuthRequestStandardModel<FormBaseConstructorModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить форму
    /// </summary>
    public Task<ResponseBaseModel> FormDeleteAsync(TAuthRequestStandardModel<FormDeleteRequestModel> req, CancellationToken cancellationToken = default);
    #endregion

    #region поля форм    
    
    /// <summary>
    /// Проверить сортировку и нормализовать в случае рассинхрона
    /// </summary>
    public Task<TResponseModel<FormConstructorModelDB>> CheckAndNormalizeSortIndexFrmFieldsAsync(int form_id, CancellationToken cancellationToken = default);
    #endregion

    /////////////// Документ. Описывается/настраивается конечный результат, который будет использоваться.
    // Может содержать одну или несколько вкладок/табов. На каждом табе/вкладке может располагаться одна или больше форм
    // Каждая располагаемая форма может быть помечена как [Табличная]. Т.е. пользователь будет добавлять сколь угодно строк одной и той же формы.
    // Пользователь при добавлении/редактировании строк таблицы будет видеть форму, которую вы настроили для этого, а внутри таба это будет выглядеть как обычная многострочная таблица с колонками, равными полям формы
    #region документы
    /// <summary>
    /// Обновить/создать схему документа
    /// </summary>
    public Task<TResponseModel<DocumentSchemeConstructorModelDB?>> UpdateOrCreateDocumentSchemeAsync(TAuthRequestStandardModel<EntryConstructedModel> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Запрос схем документов
    /// </summary>
    public Task<TPaginationResponseStandardModel<DocumentSchemeConstructorModelDB>> RequestDocumentsSchemesAsync(RequestDocumentsSchemesModel req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить схему документа
    /// </summary>
    public Task<TResponseModel<DocumentSchemeConstructorModelDB?>> GetDocumentSchemeAsync(int questionnaire_id, CancellationToken cancellationToken = default);
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
    public Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB?>> GetTabOfDocumentSchemeAsync(int questionnaire_page_id, CancellationToken cancellationToken = default);
    #endregion
    #region структура/схема таба/вкладки: формы, порядок и настройки поведения    
    /// <summary>
    /// Получить связь [таба/вкладки схемы документа] с [формой]
    /// </summary>
    public Task<TResponseModel<FormToTabJoinConstructorModelDB?>> GetTabDocumentSchemeJoinFormAsync(int questionnaire_page_join_form_id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить/создать связь [таба/вкладки схемы документа] с [формой]
    /// </summary>
    public Task<ResponseBaseModel> CreateOrUpdateTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<FormToTabJoinConstructorModelDB> req, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сдвинуть связь [таба/вкладки схемы документа] с [формой] (изменение сортировки/последовательности)
    /// </summary>
    public Task<TResponseModel<TabOfDocumentSchemeConstructorModelDB>> MoveTabDocumentSchemeJoinFormAsync(TAuthRequestStandardModel<MoveObjectModel> req, CancellationToken cancellationToken = default);
    #endregion
}