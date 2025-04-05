////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <inheritdoc/>
public class GoodsDaichiModel : GoodsDaichiBaseModel
{
    /// <summary>
    /// Параметры товара
    /// </summary>
    public ParamsGoodsDaichiModel? PARAMS { get; set; }

    /// <inheritdoc/>
    public PricesdDaichiModel? PRICES { get; set; }

    /// <summary>
    /// Информация о наличии товара на складе
    /// </summary>
    public StoreDaichiModel? STORE { get; set; }
}