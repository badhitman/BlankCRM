////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// BalanceTBankModel
/// </summary>
public class BalanceTBankModel
{
    /// <inheritdoc/>
    public decimal Balance { get; set; }

    /// <summary>
    /// Доступный остаток без учета овердрафта и с вычетом блокировок.
    /// </summary>
    public decimal RealOtb { get; set; }

    /// <summary>
    /// Доступный остаток — деньги на счете + сумма доступного овердрафта, если он подключен.
    /// </summary>
    public decimal Otb { get; set; }

    /// <summary>
    /// Сумма авторизаций — захолдированные на счете средства.
    /// </summary>
    public decimal Authorized { get; set; }

    /// <summary>
    /// Сумма платежей в картотеке клиента — собственные платежи.
    /// </summary>
    public decimal PendingPayments { get; set; }

    /// <summary>
    /// Сумма платежей в картотеке банка — требования к клиенту.
    /// </summary>
    public decimal PendingRequisitions { get; set; }
}