using System.ComponentModel.DataAnnotations;

namespace SharedLib.CommerceML2;

/// <summary>
/// Служит для определения реквизитов КО или УБР (БИК, Наименование, Адрес и т.д.) через которые клиент осуществляет расчеты
/// </summary>
public partial class Банк
{
    /// <summary>
    /// Номер счета, сформированный в соответствии со "Схемой обозначения лицевых счетов и их нумерации (по основным счетам)"
    /// приложения 1 Правил ведения бухгалтерского учета в Банке России от 18.09.97 №66
    /// или Правил ведения бухгалтерского учета в кредитных организациях, расположенных на территории РФ, от 05.12.2002 №205
    /// с учетом изменений и дополнений.
    /// </summary>
    [StringLength(20)]
    public required string СчетКорреспондентский { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public required Адрес Адрес { get; set; }

    /// <remarks/>
    public required КонтактнаяИнформация[] Контакты { get; set; }

    /// <summary>
    /// Банковский идентификационный код (БИК) в соответствии со "Справочником БИК РФ"
    /// </summary>
    public string? БИК { get; set; }

    /// <summary>
    /// Код SWIFT
    /// </summary>
    public string? SWIFT { get; set; }
}