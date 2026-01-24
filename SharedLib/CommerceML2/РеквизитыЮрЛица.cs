////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib.CommerceML2;

/// <summary>
/// Содержит описание реквизитов контрагента, специфических для юридических лиц
/// </summary>
public class РеквизитыЮрЛица
{
    /// <summary>
    /// Официальное наименование юридического лица в соответствии с учредительными документами
    /// </summary>
    public required string ОфициальноеНаименование { get; set; }

    /// <summary>
    /// Юридический адрес контрагента
    /// </summary>
    public required Адрес ЮридическийАдрес { get; set; }

    /// <summary>
    /// Индивидуальный номер налогоплательщика (ИНН) 10 цифр
    /// </summary>
    [StringLength(10, MinimumLength = 10)]
    public required string ИНН { get; set; }

    /// <summary>
    /// Код постановки предприятия на учет
    /// </summary>
    public string? КПП { get; set; }

    /// <summary>
    /// Основной вид деятельности по учредительным документам
    /// </summary>
    public string? ОсновнойВидДеятельности { get; set; }

    /// <summary>
    /// Код по единому Государственному регистру предприятий и организаций
    /// </summary>
    public string? ЕГРПО { get; set; }

    /// <summary>
    /// Код отрасли по ОКВЭД
    /// </summary>
    public string? ОКВЭД { get; set; }

    /// <summary>
    /// Код по ОКДП основного вида деятельности
    /// </summary>
    public string? ОКДП { get; set; }

    /// <summary>
    /// Код организационно-правовой формы по ОКОПФ
    /// </summary>
    public string? ОКОПФ { get; set; }

    /// <summary>
    /// Код формы собственности по ОКФС
    /// </summary>
    public string? ОКФС { get; set; }

    /// <summary>
    /// Код ОКПО организации
    /// </summary>
    public string? ОКПО { get; set; }

    /// <inheritdoc/>
    public DateOnly ДатаРегистрации { get; set; }

    /// <inheritdoc/>
    public Руководитель? Руководитель { get; set; }

    /// <remarks/>
    public required РасчетныйСчет[] РасчетныеСчета { get; set; }
}