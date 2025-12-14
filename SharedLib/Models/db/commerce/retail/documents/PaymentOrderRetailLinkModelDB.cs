////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// PaymentOrderRetailLinkModelDB
/// </summary>
[Index(nameof(OrderDocumentId), nameof(PaymentDocumentId), IsUnique = true)]
public class PaymentOrderRetailLinkModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public string? Name { get; set; }

    /// <summary>
    /// Сумма оплаты
    /// </summary>
    public decimal AmountPayment { get; set; }

    /// <inheritdoc/>
    public DocumentRetailModelDB? OrderDocument { get; set; }
    /// <inheritdoc/>
    public int OrderDocumentId { get; set; }


    /// <inheritdoc/>
    public PaymentRetailDocumentModelDB? PaymentDocument { get; set; }
    /// <inheritdoc/>
    public int PaymentDocumentId { get; set; }
}