////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Типы оплаты в рознице
/// </summary>
public enum PaymentsRetailTypesEnum
{
    /// <summary>
    /// Наличные
    /// </summary>
    Cash = 10,

    /// <summary>
    /// Картой (физически/офлайн)
    /// </summary>
    OfflineBankCard = 20,

    /// <summary>
    /// Картой (онлайн)
    /// </summary>
    OnlineBankCard = 30,

    /// <summary>
    /// QR (СБП)
    /// </summary>
    OnlineQR = 40,

    /// <summary>
    /// С внутреннего кошелька
    /// </summary>
    InternalWallet = 50,
}