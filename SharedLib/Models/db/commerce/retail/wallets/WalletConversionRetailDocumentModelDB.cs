////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Документ конвертации/перевода с кошелька на кошелёк
/// </summary>
[Index(nameof(Name)), Index(nameof(DateDocument)), Index(nameof(IsDisabled))]
public class WalletConversionRetailDocumentModelDB : EntryUpdatedModel
{
    /// <inheritdoc/>
    [Required]
    public required DateTime DateDocument { get; set; }
    
    /// <summary>
    /// Объект отключён
    /// </summary>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Кошелёк списания
    /// </summary>
    public WalletRetailModelDB? FromWallet { get; set; }

    /// <summary>
    /// Кошелёк списания
    /// </summary>
    public int FromWalletId { get; set; }

    /// <summary>
    /// Сумма списания
    /// </summary>
    public decimal FromWalletSum { get; set; }

    /// <summary>
    /// Кошелёк зачисления
    /// </summary>
    public WalletRetailModelDB? ToWallet { get; set; }

    /// <summary>
    /// Кошелёк зачисления
    /// </summary>
    public int ToWalletId { get; set; }

    /// <summary>
    /// Сумма зачисления
    /// </summary>
    public decimal ToWalletSum { get; set; }

    /// <summary>
    /// Orders
    /// </summary>
    public List<ConversionOrderRetailLinkModelDB>? Orders { get; set; }

    /// <summary>
    /// Version
    /// </summary>
    [ConcurrencyCheck]
    public Guid Version { get; set; }
}