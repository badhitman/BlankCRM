////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IConstructorBaseService
/// </summary>
public interface IConstructorBaseService
{
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
