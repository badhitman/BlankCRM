////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
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
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.