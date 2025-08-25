////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// UniversalLayerModel
/// </summary>
public class UniversalLayerModel : UniversalBaseModel
{
    /// <summary>
    /// Имя контекста для разделения различных селекторов независимо друг от друга
    /// </summary>
    public string? ContextName { get; set; }

    /// <summary>
    /// ToUpper
    /// </summary>
    public string? NormalizedNameUpper { get; set; }
}