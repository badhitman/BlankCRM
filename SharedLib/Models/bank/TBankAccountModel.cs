////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLib;

/// <summary>
/// TBankAccountModel
/// </summary>
public class TBankAccountModel
{
    /// <inheritdoc/>
    public required string AccountNumber { get; set; }

    /// <inheritdoc/>
    public required string Name { get; set; }

    /// <summary>
    /// Статус счета. Возможные значения:
    /// </summary>
    /// <remarks>
    /// BLLE — первичная блокировка договора ЮЛ.Находится в этом статусе до момента активации (переход в NORM) или закрытия(переход в CLSC/CLSB).
    /// ARSS — частичный арест.
    /// ARSF — полный арест.
    /// ARMF — полная блокировка.
    /// CAIN — картотека.
    /// CLBL — в процессе закрытия.
    /// CLSC — закрыт по инициативе клиента.
    /// CLSB — закрыт по инициативе банка.
    /// </remarks>
    public required string Status { get; set; }

    /// <inheritdoc/>
    public required string TariffName { get; set; }

    /// <inheritdoc/>
    public required string TariffCode { get; set; }

    /// <summary>
    /// Код валюты счета по ОКВ (цифрами).
    /// </summary>
    public required string Currency { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Флаг основного счета: Y, N
    /// </summary>
    public required string MainFlag { get; set; }

    /// <inheritdoc/>
    public required string BankBik { get; set; }

    /// <summary>
    /// Тип счета. Список вариантов значений может пополняться.
    /// </summary>
    /// <remarks>
    /// Current — расчетный счет.
    /// Tax — счет Т-Бухгалтерии.
    /// Tender — специальный счет для участия в госзакупках.
    /// Overnight — счет Overnight.
    /// Trust — специальный счет доверительного управляющего ПИФ.
    /// Broker — специальный брокерский счет.
    /// BankPaymentAgent — специальный счет банковского платежного агента.
    /// PaymentAgent — счет платежного агента.
    /// Nominal — номинальный счет.
    /// NominalIpo — номинальный счет оператора инвестиционной платформы.
    /// TrustManagementSmp — специальный счет доверительного управления.
    /// Cashbox — бизнес-копилка.
    /// Invest — инвестиционный счет.
    /// </remarks>
    public required string AccountType { get; set; }

    /// <inheritdoc/>
    public DateTime? ActivationDate { get; set; }

    /// <inheritdoc/>
    [NotMapped]
    public BalanceTBankModel? Balance { get; set; }
}