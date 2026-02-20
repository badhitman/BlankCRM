////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Собирательный компонент для всего, что может быть упомянуто в процессе обмена
/// </summary>
public partial class КоммерческаяИнформация
{
    /// <remarks/>
    public required Классификатор Классификатор { get; set; }

    /// <remarks/>
    public Документ[]? Документ { get; set; }

    /// <remarks/>
    public ИзмененияПакетаПредложений? ИзмененияПакетаПредложений { get; set; }

    /// <remarks/>
    public Каталог? Каталог { get; set; }

    /// <remarks/>
    public ПакетПредложений? ПакетПредложений { get; set; }

    /// <remarks/>
    public required string ВерсияСхемы { get; set; }

    /// <remarks/>
    public required DateTime ДатаФормирования { get; set; }
}