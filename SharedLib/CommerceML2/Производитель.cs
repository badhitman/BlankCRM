////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Содержит описание страны, непосредственного изготовителя и торговой марки товара
/// </summary>
public class Производитель
{
    /// <remarks/>
    public string? Страна { get; set; }

    /// <remarks/>
    public string? ТорговаяМарка { get; set; }

    /// <remarks/>
    public Контрагент? ВладелецТорговойМарки { get; set; }

    /// <remarks/>
    public Контрагент? Изготовитель { get; set; }
}