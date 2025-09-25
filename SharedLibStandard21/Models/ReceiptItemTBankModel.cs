////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// Данные о позиции чека
/// </summary>
public class ReceiptItemTBankModel
{
    /// <summary>
    /// Наименование товара
    /// </summary>
    [JsonProperty(PropertyName = "Name", Required = Required.Always)]
    public string? Name { get; set; }

    /// <summary>
    /// Количество или вес товара
    /// </summary>
    [JsonProperty(PropertyName = "Quantity", Required = Required.Always)]
    public uint Quantity { get; set; }

    /// <summary>
    /// Цена за единицу товара в копейках
    /// </summary>
    [JsonProperty(PropertyName = "Price", Required = Required.Always)]
    public uint Price { get; set; }

    /// <summary>
    /// Ставка НДС
    /// </summary>
    [JsonProperty(PropertyName = "Tax", Required = Required.Always)]
    public ETaxesEnum Tax { get; set; }

    /// <summary>
    /// Стоимость товара в копейках, произведение <see cref="Quantity"/> и <see cref="Price"/>
    /// </summary>
    public uint Amount => Price * Quantity;

    /// <summary>
    /// Штрих-код в требуемом формате. В зависимости от типа кассы требования могут отличаться:
    /// <list type="bullet">
    /// <item>АТОЛ Онлайн - шестнадцатеричное представление с пробелами. Максимальная длина – 32 байта. Пример: 00 00 00 01 00 21 FA 41 00 23 05 41 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 12 00 AB 00</item>
    /// <item>CloudKassir - длина строки: четная, от 8 до 150 байт, т.е. от 16 до 300 ASCII символов ['0' - '9' , 'A' - 'F' ] шестнадцатеричного представления кода маркировки товара. Пример: 303130323930303030630333435</item>
    /// <item>OrangeData - строка, содержащая base64 кодированный массив от 8 до 32 байт. Пример: igQVAAADMTIzNDU2Nzg5MDEyMwAAAAAAAQ==</item>
    /// </list>
    /// В случае передачи в запросе параметра Ean13 не прошедшего валидацию, возвращается неуспешный ответ с текстом ошибки в параметре message = "Неверный параметр Ean13". Валидация параметра Ean13 необходима как в объекте Receipt, так и в объекте Receipts.
    /// </summary>
    public string? Ean13 { get; set; }

    /// <summary>
    /// Признак способа расчета
    /// </summary>
    public EPaymentMethodsEnum? PaymentMethod { get; set; }

    /// <summary>
    /// Признак предмета расчета
    /// </summary>
    public EPaymentObjectsEnum? PaymentObject { get; set; }

    /// <summary>
    /// Данные агента. Используется при работе по агентской схеме.
    /// </summary>
    public virtual AgentDataForReceiptItemTBankModel? AgentData { get; set; }

    /// <summary>
    /// Данные поставщика платежного агента.
    /// Используется при работе по агентской схеме.
    /// Требуется передать в случае если задан <see cref="AgentData"/>
    /// </summary>
    public virtual SupplierInfoForReceiptItemTBankModel? SupplierInfo { get; set; }
}