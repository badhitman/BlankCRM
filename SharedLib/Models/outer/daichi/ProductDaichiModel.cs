////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <inheritdoc/>
public class ProductDaichiModel : ProductDaichiBaseModel
{
    /// <summary>
    /// Параметры товара
    /// </summary>
    public required ParamsProductDaichiModel PARAMS { get; set; }

    /// <inheritdoc/>
    public required PricesdDaichiModel PRICES { get; set; }

    /// <summary>
    /// Информация о наличии товара на складе
    /// </summary>
    public required StoreDaichiModel STORE { get; set; }
}