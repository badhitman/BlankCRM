////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Типы оплаты в рознице
/// </summary>
public enum PaymentsRetailTypesEnumObsolete
{
    /// <summary>
    /// Онлайн (картой)
    /// </summary>
    [Description("Онлайн (картой)")]
    OnlineBankCard = 0,

    /// <summary>
    /// Терминал (картой офлайн)
    /// </summary>
    [Description("Терминал (картой офлайн)")]
    OfflineBankCard = 20,

    /// <summary>
    /// Наличные
    /// </summary>
    [Description("Наличные")]
    Cash = 40,

    /// <summary>
    /// На сайте
    /// </summary>
    [Description("На сайте")]
    OnSite = 60,

    /// <summary>
    /// QR (СБП)
    /// </summary>
    [Description("QR (СБП)")]
    OnlineQR = 80,
}