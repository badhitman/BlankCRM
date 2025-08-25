////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// TagViewModel
/// </summary>
public class TagViewModel : StorageBaseModel
{
    /// <summary>
    /// NormalizedTagNameUpper
    /// </summary>
    public  string? NormalizedTagNameUpper { get; set; }

    /// <summary>
    /// Имя параметра
    /// </summary>
    public  string? TagName { get; set; }
}