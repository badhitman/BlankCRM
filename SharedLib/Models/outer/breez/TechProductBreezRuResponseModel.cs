////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Технические характеристики Продукта
/// </summary>
/// <remarks>
/// В ответ на данный запрос возвращаются данные по продукту с указанным идентификатором.
/// Данные включают в себя НС-коды, а также все технические характеристики продукта и их значения.
/// В случае, если продукта с указанным идентификатором не существует, в JSON возвращается объект с ключом "error" и значением "Неверный ID",
/// а в XML - пустой объект "product".
/// </remarks>
public class TechProductBreezRuResponseModel : ProductBreezRuLiteModel
{
    /// <summary>
    /// Объект, содержащий ТХи, где ключ - идентификатор ТХ, а значение - объект с ТХ
    /// </summary>
    public Dictionary<int, PropTechProductBreezRuModel>? Techs { get; set; }

    /// <inheritdoc/>
    public static TechProductBreezRuResponseModel Build(KeyValuePair<string, TechProductBreezRuResponseModel> x)
    {
        return new()
        {
            AccessoryNC = x.Value.AccessoryNC,
            NarujNC = x.Value.NarujNC,
            NC = x.Value.NC,
            VnutrNC = x.Value.VnutrNC,
            Techs = x.Value.Techs,
        };
    }
}