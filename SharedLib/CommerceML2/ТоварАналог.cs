namespace SharedLib.CommerceML2;

/// <summary>
/// Элементы типа «Товар» - определяют аналогичные товары, например, в другом каталоге
/// </summary>
public partial class ТоварАналог : Товар
{
    /// <summary>
    /// Идентификатор каталога
    /// </summary>
    public required string ИдКаталога { get; set; }

    /// <summary>
    /// Идентификатор классификатора, в соответствии с которым описан аналог
    /// </summary>
    public required string ИдКлассификатора { get; set; }
}