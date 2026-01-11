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
public class ConversionOrderRetailLinkModelDB : OrderConversionAmountModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public DocumentRetailModelDB? OrderDocument { get; set; }

    /// <inheritdoc/>
    public WalletConversionRetailDocumentModelDB? ConversionDocument { get; set; }


    /// <inheritdoc/>
    public static ConversionOrderRetailLinkModelDB Build(OrderConversionAmountModel req)
    {
        return new()
        {
            AmountPayment = req.AmountPayment,
            ConversionDocumentId = req.ConversionDocumentId,
            OrderDocumentId = req.OrderDocumentId,
        };
    }
}