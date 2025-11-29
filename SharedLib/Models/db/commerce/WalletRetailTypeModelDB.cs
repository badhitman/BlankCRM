////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Тип кошелька
/// </summary>
public class WalletRetailTypeModelDB : EntryUpdatedModel
{
    /// <summary>
    /// Кошельки
    /// </summary>
    public List<WalletRetailModelDB>? Wallets { get; set; }
}