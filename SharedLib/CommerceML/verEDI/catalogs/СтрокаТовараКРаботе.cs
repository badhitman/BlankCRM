////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <summary>
/// Информация о единице товара, принятой клиентом к работе во взаимодействии с поставщиком
/// </summary>
public partial class СтрокаТовараКРаботе
{
    /// <remarks/>
    public required string ИдТовараКлиента { get; set; }

    /// <remarks/>
    public required string ИдТовараПоставщика { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public decimal? КоэффициентПересчета { get; set; }

    /// <remarks/>
    public string? Примечание { get; set; }
}
