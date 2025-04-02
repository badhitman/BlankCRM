////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <inheritdoc/>
public class GoodsDaichiModel : DaichiEntryModel
{
    /// <inheritdoc/>
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int KeyIndex { get; set; }

    /// <summary>
    /// Параметры товара
    /// </summary>
    public ParamsDaichiModel? PARAMS { get; set; }

    /// <inheritdoc/>
    public PricesdDaichiModel? PRICES { get; set; }

    /// <summary>
    /// Информация о наличии товара на складе
    /// </summary>
    public StoreDaichiModel? STORE { get; set; }
}