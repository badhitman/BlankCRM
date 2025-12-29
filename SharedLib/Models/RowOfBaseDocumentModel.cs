////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RowOfBaseDocumentModel
/// </summary>
public abstract class RowOfBaseDocumentModel : RowOfSimpleDocumentModel
{
    /// <summary>
    /// Вес
    /// </summary>
    public decimal WeightOffer { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{base.ToString()} (w:{WeightOffer})";
    }
}