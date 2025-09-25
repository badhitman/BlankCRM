////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SharedLib;

/// <summary>
/// EStatusResponsesEnum
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StatusResponsesTBankEnum
{
    /// <summary>
    /// Создан
    /// </summary>
    [EnumMember(Value = "NEW")]
    New = 1,
    /// <summary>
    /// Платежная форма открыта покупателем
    /// </summary>
    [EnumMember(Value = "FORM_SHOWED")]
    FormShowed,
    /// <summary>
    /// Просрочен
    /// </summary>
    [EnumMember(Value = "DEADLINE_EXPIRED")]
    DeadlineExpired,
    /// <summary>
    /// Отменен
    /// </summary>
    [EnumMember(Value = "CANCELED")]
    Canceled,
    /// <summary>
    /// Проверка платежных данных. Промежуточный.
    /// </summary>
    [EnumMember(Value = "PREAUTHORIZING")]
    Preauthorizing,
    /// <summary>
    /// Резервируется. Промежуточный.
    /// </summary>
    [EnumMember(Value = "AUTHORIZING")]
    Authorizing,
    /// <summary>
    /// Зарезервирован
    /// </summary>
    [EnumMember(Value = "AUTHORIZED")]
    Authorized,
    /// <summary>
    /// Не прошел авторизацию. Промежуточный.
    /// </summary>
    [EnumMember(Value = "AUTH_FAIL")]
    AuthFail,
    /// <summary>
    /// Отклонен
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    Rejected,
    /// <summary>
    /// Проверяется по протоколу 3-D Secure
    /// </summary>
    [EnumMember(Value = "3DS_CHECKING")]
    ThreeDSChecking,
    /// <summary>
    /// Проверен по протоколу 3-D Secure. Промежуточный.
    /// </summary>
    [EnumMember(Value = "3DS_CHECKED")]
    ThreeDSChecked,
    /// <summary>
    /// Резервирование отменяется. Промежуточный.
    /// </summary>
    [EnumMember(Value = "REVERSING")]
    Reversing,
    /// <summary>
    /// Резервирование отменено частично
    /// </summary>
    [EnumMember(Value = "PARTIAL_REVERSED")]
    PartialReversed,
    /// <summary>
    /// Резервирование отменено
    /// </summary>
    [EnumMember(Value = "REVERSED")]
    Reversed,
    /// <summary>
    /// Подтверждается. Промежуточный.
    /// </summary>
    [EnumMember(Value = "CONFIRMING")]
    Confirming,
    /// <summary>
    /// Подтвержден
    /// </summary>
    [EnumMember(Value = "CONFIRMED")]
    Confirmed,
    /// <summary>
    /// Возвращается. Промежуточный.
    /// </summary>
    [EnumMember(Value = "REFUNDING")]
    Refunding,
    /// <summary>
    /// Возвращен частично
    /// </summary>
    [EnumMember(Value = "PARTIAL_REFUNDED")]
    PartialRefunded,
    /// <summary>
    /// Возвращен полностью
    /// </summary>
    [EnumMember(Value = "REFUNDED")]
    Refunded,
}