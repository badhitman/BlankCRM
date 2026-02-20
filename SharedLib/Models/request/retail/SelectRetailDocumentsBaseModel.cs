////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// SelectRetailDocumentsBaseModel
/// </summary>
public class SelectRetailDocumentsBaseModel : PeriodBaseModel
{
    /// <summary>
    /// Только заказы с рваными сумами
    /// </summary>
    public bool? EqualsSumFilter { get; set; }
}