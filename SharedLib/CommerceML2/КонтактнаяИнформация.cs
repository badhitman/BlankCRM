////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Содержит описание возможных способов связи (по телефону, электронной почте и т.д.)
/// </summary>
public partial class КонтактнаяИнформация
{
    /// <remarks/>
    public КонтактТип Тип { get; set; }

    /// <remarks/>
    public string? Значение { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }
}