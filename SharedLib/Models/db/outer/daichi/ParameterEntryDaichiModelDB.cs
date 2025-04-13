////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// ParameterEntryDaichiModelDB
/// </summary>
[Index(nameof(CreatedAt)), Index(nameof(UpdatedAt))]
public class ParameterEntryDaichiModelDB : ParameterElementDaichiModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public List<SectionParameterDaichiModelDB>? Sections { get; set; }

    /// <inheritdoc/>
    public List<PhotoParameterDaichiModelDB>? Photos { get; set; }

    /// <inheritdoc/>
    public List<AttributeParameterDaichiModelDB>? Attributes { get; set; }

    /// <summary>
    /// Дата первого появления
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }


    /// <inheritdoc/>
    public static ParameterEntryDaichiModelDB Build(ParameterElementDaichiJsonModel x, List<ProductDaichiModelDB> productsDb)
    {
        ParameterEntryDaichiModelDB res = new()
        {
            NAME = x.NAME,
            XML_ID = x.XML_ID,
            ID = x.ID,
            BRAND = x.BRAND,
            MAIN_SECTION = x.MAIN_SECTION,
        };

        if (x.SECTIONS is not null && x.SECTIONS.Length != 0)
            res.Sections = [.. x.SECTIONS.Select(y => new SectionParameterDaichiModelDB() { Name = y, Parent = res })];

        if (x.PHOTOES is not null && x.PHOTOES.Length != 0)
            res.Photos = [.. x.PHOTOES.Select(y => new PhotoParameterDaichiModelDB() { Name = y, Parent = res })];

        if (x.Attributes is not null && x.Attributes.Count != 0)
            res.Attributes = [.. x.Attributes.Select(y => AttributeParameterDaichiModelDB.Build(y, res))];

        return res;
    }

    /// <inheritdoc/>
    public void SetLive()
    {
        Attributes?.ForEach(pi => { pi.Parent = this; });
        Photos?.ForEach(pp => { pp.Parent = this; });
        Sections?.ForEach(pp => { pp.Parent = this; });
    }
}