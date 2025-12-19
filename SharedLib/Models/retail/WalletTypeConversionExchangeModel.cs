////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// WalletTypeConversionExchangeModel
/// </summary>
public class WalletTypeConversionExchangeModel
{
    /// <summary>
    /// Тип кошелька
    /// </summary>
    public int WalletTypeSenderId { get; set; }

    /// <summary>
    /// Тип кошелька
    /// </summary>
    public int WalletTypeRecipientId { get; set; }

    /// <summary>
    /// Курс обмена
    /// </summary>
    public decimal ExchangeRate { get; set; }
}