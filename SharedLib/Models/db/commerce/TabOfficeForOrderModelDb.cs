////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Адрес организации в заказе
/// </summary>
[Index(nameof(WarehouseId))]
public class TabOfficeForOrderModelDb
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// OrderDocument
    /// </summary>
    public OrderDocumentModelDB? Order { get; set; }

    /// <summary>
    /// OrderDocument
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// OfficeOrganization
    /// </summary>
    public OfficeOrganizationModelDB? Office { get; set; }

    /// <summary>
    /// OfficeOrganization
    /// </summary>
    public int OfficeId { get; set; }

    /// <summary>
    /// Склад
    /// </summary>
    public int WarehouseId { get; set; }

    /// <summary>
    /// Строки заказа
    /// </summary>
    public List<RowOfOrderDocumentModelDB>? Rows { get; set; }
}