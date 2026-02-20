////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <summary>
/// Базовый тип идентификаторов контрагента
/// </summary>
public partial class ИдентификаторКонтрагента
{
    /// <summary>
    /// Код в справочнике Ассоциации автоматической идентификации GS1 Global Location Number
    /// </summary>
    public string? GLN { get; set; }

    /// <summary>
    /// Номер налогоплательщика РФ
    /// </summary>
    public string? ИННРФ { get; set; }

    /// <summary>
    /// Идентификатор контрагента, представленный в виде UUID.
    /// </summary>
    /// <remarks>
    /// Universally Unique Identifier (http://www.opengroup.org/dce/info/draft-leach-uuids-guids-01.txt)
    /// </remarks>
    public string? КонтрагентИД { get; set; }
}