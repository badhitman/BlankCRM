////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Наличие товара на складе
/// </summary>
public class AvailabilityProductsDaichiModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }


    /// <inheritdoc/>
    public ProductDaichiModelDB? Product { get; set; }
    /// <inheritdoc/>
    public int ProductId { get; set; }


    /// <inheritdoc/>
    public StoreDaichiModelDB? Store { get; set; }
    /// <inheritdoc/>
    public int StoreId { get; set; }

    /// <summary>
    /// Количество свободного (не забронированного) товара на складе (‘На складе’)
    /// </summary>
    public int STORE_AMOUNT { get; set; }
    /// <summary>
    /// Количество товара, которое может быть доставлено на склад в течении 14 дней (‘В пути’)
    /// </summary>
    public int DELIVERY_AMOUNT { get; set; }

    /// <summary>
    /// Количество, больше которого не показывается точное значение. При превышении ограничения на портале точное значение заменяется выражением ‘>’
    /// </summary>
    public int? STORE_LIMIT { get; set; }
    /// <summary>
    /// Количество, больше которого не показывается точное значение. При превышении ограничения на портале точное значение заменяется выражением ‘>’
    /// </summary>
    public int? DELIVERY_LIMIT { get; set; }


    /// <summary>
    /// false (лимит не превышен. STORE_AMOUNT показывает точное значение)
    /// true (лимит превышен. STORE_AMOUNT ограничено значением LIMIT)
    /// </summary>
    public bool? STORE_HIDE_MORE_LIMIT { get; set; }
    /// <summary>
    /// false (лимит не превышен. DELIVERY_AMOUNT показывает точное значение)
    /// true (лимит превышен. DELIVERY_AMOUNT ограничено значением LIMIT)
    /// </summary>
    public bool? DELIVERY_HIDE_MORE_LIMIT { get; set; }
}