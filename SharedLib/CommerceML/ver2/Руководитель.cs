////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <remarks/>
public partial class Руководитель
{
    /// <remarks/>
    public required string Фамилия { get; set; }

    /// <remarks/>
    public required string Имя { get; set; }

    /// <remarks/>
    public string? Отчество { get; set; }

    /// <remarks/>
    public УдостоверениеЛичности? УдостоверениеЛичности { get; set; }

    /// <remarks/>
    public Адрес? АресРегистрации { get; set; }

    /// <remarks/>
    public required string Должность { get; set; }

    /// <remarks/>
    public required КонтактнаяИнформация[] Контакты { get; set; }
}