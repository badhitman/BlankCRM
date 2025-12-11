////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// RetailDeliveryOrderLinkModelDB
/// </summary>
[Index(nameof(OrderDocumentId), nameof(DeliveryDocumentId), IsUnique = true)]
public class RetailDeliveryOrderLinkModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Вес отправления
    /// </summary>
    public decimal WeightShipping { get; set; }

    /// <inheritdoc/>
    public RetailDocumentModelDB? OrderDocument { get; set; }
    /// <inheritdoc/>
    public int OrderDocumentId { get; set; }


    /// <inheritdoc/>
    public DeliveryDocumentRetailModelDB? DeliveryDocument { get; set; }
    /// <inheritdoc/>
    public int DeliveryDocumentId { get; set; }
}