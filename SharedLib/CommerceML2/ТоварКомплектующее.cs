////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Элементы типа «Товар» - определяют комплектующие составных товаров - наборов.
/// </summary>
public partial class ТоварКомплектующее : Товар
{
    /// <summary>
    /// Идентификатор каталога
    /// </summary>
    public required string ИдКаталога { get; set; }

    /// <summary>
    /// Идентификатор классификатора, в соответствии с которым описано комплектующее
    /// </summary>
    public required string ИдКлассификатора { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }
}