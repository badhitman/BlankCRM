////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// ConversionOrderRetailLinkModelDB
/// </summary>
[Index(nameof(OrderDocumentId), nameof(ConversionDocumentId), IsUnique = true)]
public class ConversionOrderRetailLinkModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Сумма оплаты
    /// </summary>
    public decimal AmountPayment { get; set; }

    /// <inheritdoc/>
    public DocumentRetailModelDB? OrderDocument { get; set; }
    /// <inheritdoc/>
    public int OrderDocumentId { get; set; }


    /// <inheritdoc/>
    public WalletConversionRetailDocumentModelDB? ConversionDocument { get; set; }
    /// <inheritdoc/>
    public int ConversionDocumentId { get; set; }
}