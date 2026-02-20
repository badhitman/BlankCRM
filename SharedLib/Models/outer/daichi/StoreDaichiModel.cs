////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public class StoreDaichiModel : DaichiEntryModel
{
    /// <summary>
    /// Количество свободного (не забронированного) товара на складе (‘На складе’)
    /// </summary>
    public int STORE_AMOUNT { get; set; }

    /// <summary>
    /// Количество товара, которое может быть доставлено на склад в течении 14 дней (‘В пути’)
    /// </summary>
    public int DELIVERY_AMOUNT { get; set; }

    /// <summary>
    /// Ограничение на информацию о доступности товара на складе	
    /// </summary>
    public StoreParamsInfoDaichiModel? STORE_PARAMS_INFO { get; set; }

    /// <summary>
    /// Ограничение на информацию о доступности товара в пути	
    /// </summary>
    public StoreParamsInfoDaichiModel? DELIVERY_PARAMS_INFO { get; set; }
}