////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Документ конвертации/перевода с кошелька на кошелёк
/// </summary>
public class WalletConversionDocumentModelDB : EntryUpdatedModel
{
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
}