////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib.CommerceML2;

/// <summary>
/// Номер расчетного счета контрагента
/// </summary>
public partial class РасчетныйСчет
{
    /// <summary>
    /// Номер счета, сформированный в соответствии со "Схемой обозначения лицевых счетов и их нумерации (по основным счетам)"
    /// приложения 1 Правил ведения бухгалтерского учета в Банке России от 18.09.97 №66
    /// или Правил ведения бухгалтерского учета в кредитных организациях, расположенных на территории РФ, от 05.12.2002 №205
    /// с учетом изменений и дополнений.
    /// </summary>
    [StringLength(20)]
    public required string НомерСчета { get; set; }

    /// <remarks/>
    public required Банк Банк { get; set; }

    /// <remarks/>
    public required Банк БанкКорреспондент { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }
}