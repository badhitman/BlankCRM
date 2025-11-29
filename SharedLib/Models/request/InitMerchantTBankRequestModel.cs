////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// InitMerchantTBankRequestModel
/// </summary>
public class InitMerchantTBankRequestModel
{
    /// <inheritdoc/>
    public required string PayerUserId { get; set; }

    /// <inheritdoc/>
    public bool IsRecurrent { get; set; }

    /// <inheritdoc/>
    public required uint Amount { get; set; }

    /// <summary>
    /// IP-адрес покупателя	
    /// </summary>
    public string? IP { get; set; }

    /// <inheritdoc/>
    public required int OrderId { get; set; }

    /// <summary>
    /// Описание заказа
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Данные по чеку
    /// </summary>
    public required ReceiptTBankModel Receipt { get; set; }

    /// <summary>
    /// Дополнительные параметры платежа в формате "ключ":"значение" (не более 20 пар). Наименование самого параметра должно быть в верхнем регистре, иначе его содержимое будет игнорироваться.
    /// <list type="number">
    /// <item> Если у терминала включена опция привязки покупателя после успешной оплаты и передается параметр CustomerKey, то в передаваемых параметрах DATA могут присутствовать параметры метода AddCustomer. Если они присутствуют, то автоматически привязываются к покупателю.
    /// Например, если указать: "DATA":{"Phone":"+71234567890", "Email":"a@test.com"}, к покупателю автоматически будут привязаны данные Email и телефон, и они будут возвращаться при вызове метода GetCustomer.</item>
    /// <item>
    /// Если используется функционал сохранения карт на платежной форме, то при помощи опционального параметра "DefaultCard" можно задать какая карта будет выбираться по умолчанию. Возможные варианты:
    /// <list type="bullet">
    /// <item>Оставить платежную форму пустой. Пример: "DATA":{"DefaultCard":"none"};</item>
    /// <item>Заполнить данными передаваемой карты. В этом случае передается CardId. Пример: "DATA":{"DefaultCard":"894952"};</item>
    /// <item>Заполнить данными последней сохраненной карты. Применяется, если параметр "DefaultCard" не передан, передан с некорректным значением или в значении null.</item>
    /// </list>
    /// По умолчанию возможность сохранения карт на платежной форме может быть отключена. Для активации обратитесь в службу технической поддержки.
    /// </item>
    /// </list>
    /// </summary>
    public Dictionary<string, string>? Data { get; set; }

    /// <summary>
    /// Cрок жизни ссылки (не более 90 дней)
    /// </summary>
    [JsonConverter(typeof(TBankDateTimeConverter))]
    public DateTime? RedirectDueDate { get; set; }

    /// <summary>
    /// Адрес для получения http нотификаций
    /// </summary>
    public string? NotificationURL { get; set; }

    /// <summary>
    /// Страница успеха
    /// </summary>
    public string? SuccessURL { get; set; }

    /// <summary>
    /// Страница ошибки
    /// </summary>
    public string? FailURL { get; set; }

    /// <summary>
    /// Язык платежной формы
    /// </summary>
    public LanguageFormTBankEnum? Language { get; set; } = LanguageFormTBankEnum.Ru;

    /// <summary>
    /// Тип оплаты, одно или дву стадийная
    /// </summary>
    public PayTypesTBankEnum? PayType { get; set; }

    /// <summary>
    /// Если указать тип QR кода (СБП/НСПК), тогда он будет запрошен вместе с инициацией платежа
    /// </summary>
    /// <remarks>
    /// если не установлено, тогда QR код (СБП/НСПК) не будет сформирован
    /// </remarks>
    public DataTypeQREnum? GenerateQR {  get; set; }

    /// <summary>
    /// Creator/Initiator (user id)
    /// </summary>
    public required string InitiatorUserId { get; set; }
}