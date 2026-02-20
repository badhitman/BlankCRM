////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SharedLib;

/// <summary>
/// Типы оплаты, одно или дву стадийная
/// </summary>
/// <remarks>
/// Определяет тип проведения платежа.
/// Если параметр передан, используется его значение, если нет — значение из настроек терминала.
/// </remarks>
[JsonConverter(typeof(StringEnumConverter))]
public enum PayTypesTBankEnum
{
    /// <summary>
    /// Одностадийная
    /// </summary>
    [EnumMember(Value = "O")]
    OneStagePayment = 1,

    /// <summary>
    /// Двух-стадийная
    /// </summary>
    [EnumMember(Value = "T")]
    TwoStagePayment
}