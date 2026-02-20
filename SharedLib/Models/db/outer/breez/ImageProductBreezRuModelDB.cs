////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ImageProductBreezRuModelDB
/// </summary>
public class ImageProductBreezRuModelDB : EntryStandardModel
{
    /// <summary>
    /// Product
    /// </summary>
    public ProductBreezRuModelDB? Product { get; set; }
    /// <summary>
    /// Product
    /// </summary>
    public int ProductId { get; set; }
}