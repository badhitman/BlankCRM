////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// RowOfSimpleDocumentModel
/// </summary>
public abstract class RowOfSimpleDocumentModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Торговое предложение
    /// </summary>
    public OfferModelDB? Offer { get; set; }

    /// <summary>
    /// Offer
    /// </summary>
    public int OfferId { get; set; }

    /// <summary>
    /// Номенклатура
    /// </summary>
    public NomenclatureModelDB? Nomenclature { get; set; }

    /// <summary>
    /// Nomenclature
    /// </summary>
    public int NomenclatureId { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"#{Id} '{Offer?.GetName()}'";
    }
}