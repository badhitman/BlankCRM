////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// ParameterEntryDaichiModelDB
/// </summary>
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

        if(x.Attributes is not null && x.Attributes.Count != 0)
        res.Attributes = [..x.Attributes.Select(y => AttributeParameterDaichiModelDB.Build(y,res))];

        return res;
    }
}