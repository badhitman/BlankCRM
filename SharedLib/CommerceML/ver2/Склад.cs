////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Наименование и идентификатор склада
/// </summary>
public partial class Склад
{
    /// <summary>
    /// Глобально уникальный идентификатор склада (рекомендуется использовать GUID)
    /// </summary>
    public required string Ид { get; set; }

    /// <summary>
    /// Наименование склада
    /// </summary>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }

    /// <summary>
    /// Фактический адрес склада
    /// </summary>
    public Адрес? Адрес { get; set; }

    /// <summary>
    /// Содержит описание возможных способов связи (по телефону, электронной почте и т.д.)
    /// </summary>
    public required КонтактнаяИнформация[] Контакты { get; set; }
}