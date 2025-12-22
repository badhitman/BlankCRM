////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// SelectRetailDocumentsRequestModel
/// </summary>
public class SelectRetailDocumentsBaseModel
{
    /// <inheritdoc/>
    public List<StatusesDocumentsEnum?>? StatusesFilter { get; set; }

    /// <summary>
    /// Только заказы с рваными сумами
    /// </summary>
    public bool? EqualsSumFilter { get; set; }

    /// <inheritdoc/>
    public DateTime? Start { get; set; }
    /// <inheritdoc/>
    public DateTime? End { get; set; }
}