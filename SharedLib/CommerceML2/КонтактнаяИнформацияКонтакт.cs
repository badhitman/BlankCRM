namespace SharedLib.CommerceML2;

/// <summary>
/// Содержит описание возможных способов связи (по телефону, электронной почте и т.д.)
/// </summary>
public partial class КонтактнаяИнформацияКонтакт
{
    /// <remarks/>
    public КонтактТип Тип { get; set; }

    /// <remarks/>
    public string? Значение { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }
}