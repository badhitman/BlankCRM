////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Изменения публикуемых предложений - для быстрой загрузки на сайт
/// </summary>
public partial class ИзмененияПакетаПредложений
{
    /// <summary>
    /// Идентификатор Пакета
    /// </summary>
    public required string Ид { get; set; }

    /// <summary>
    /// Идентификатор каталога, по которому составлен Пакет
    /// </summary>
    public required string ИдКаталога { get; set; }

    /// <remarks/>
    public required ИзмененияПакетаПредложенийПредложение[] Предложения { get; set; }

    /// <remarks/>
    public bool? СодержитТолькоИзменения { get; set; }
}