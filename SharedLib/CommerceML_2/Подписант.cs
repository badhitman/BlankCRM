////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <remarks/>
public partial class Подписант
{
    /// <remarks/>
    public required string Фамилия { get; set; }

    /// <remarks/>
    public required string Имя { get; set; }

    /// <remarks/>
    public string? Отчество { get; set; }

    /// <summary>
    /// Например: Г-н, Г-жа, Докт., Проф. и т.д.
    /// </summary>
    public string? Обращение { get; set; }

    /// <remarks/>
    public УдостоверениеЛичности? УдостоверениеЛичности { get; set; }

    /// <remarks/>
    public Адрес? АдресРегистрации { get; set; }

    /// <remarks/>
    public РеквизитыЮрЛица? МестоРаботы { get; set; }

    /// <remarks/>
    public string? Должность { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }
}