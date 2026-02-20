////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// ProductBreezRuModel
/// </summary>
public class ProductBreezRuModel : ProductBreezRuBaseModel
{
    /// <summary>
    /// Информация о цене
    /// </summary>
    public Dictionary<string, string>? Price { get; set; }

    /// <summary>
    /// URL изображений продукта
    /// </summary>
    public string[]? Images { get; set; }
}