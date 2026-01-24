////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Содержит описание реквизитов контрагента, специфических для физических лиц
/// </summary>
public class РеквизитыФизЛица
{
    /// <remarks/>
    public required string ПолноеНаименование { get; set; }

    /// <summary>
    /// Например: Г-н, Г-жа, Докт., Проф. и т.д.
    /// </summary>
    public string? Обращение { get; set; }

    /// <remarks/>
    public required string Фамилия { get; set; }

    /// <remarks/>
    public required string Имя { get; set; }

    /// <remarks/>
    public string? Отчество { get; set; }

    /// <remarks/>
    public required DateOnly ДатаРождения { get; set; }

    /// <remarks/>
    public Адрес? МестоРождения { get; set; }

    /// <remarks/>
    public ПолТип? Пол { get; set; }

    /// <summary>
    /// Индивидуальный номер налогоплательщика (ИНН) 10 цифр
    /// </summary>
    public string? ИНН { get; set; }

    /// <summary>
    /// Код постановки предприятия на учет
    /// </summary>
    public string? КПП { get; set; }

    /// <remarks/>
    public УдостоверениеЛичности? УдостоверениеЛичности { get; set; }

    /// <remarks/>
    public Адрес? АдресРегистрации { get; set; }

    /// <remarks/>
    public РеквизитыЮрЛица? МестоРаботы { get; set; }
}