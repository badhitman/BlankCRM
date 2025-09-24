////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SharedLib;

/// <summary>
/// Предмет расчета
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum EPaymentObjectsEnum
{
    /// <summary>
    /// Товар
    /// </summary>
    [EnumMember(Value = "commodity")]
    Commodity = 1,
    /// <summary>
    /// Подакцизный товар
    /// </summary>
    [EnumMember(Value = "excise")]
    Excise,
    /// <summary>
    /// Работа
    /// </summary>
    [EnumMember(Value = "job")]
    Job,
    /// <summary>
    /// Услуга
    /// </summary>
    [EnumMember(Value = "service")]
    Service,
    /// <summary>
    /// Ставка азартной игры
    /// </summary>
    [EnumMember(Value = "gambling_bet")]
    GamblingBet,
    /// <summary>
    /// Выигрыш азартной игры
    /// </summary>
    [EnumMember(Value = "gambling_prize")]
    GamblingPrize,
    /// <summary>
    /// Лотерейный билет
    /// </summary>
    [EnumMember(Value = "lottery")]
    Lottery,
    /// <summary>
    /// Выигрыш лотереи
    /// </summary>
    [EnumMember(Value = "lottery_prize")]
    LotteryPrize,
    /// <summary>
    /// Предоставление результатов интеллектуальной деятельности
    /// </summary>
    [EnumMember(Value = "intellectual_activity")]
    IntellectualActivity,
    /// <summary>
    /// Платеж
    /// </summary>
    [EnumMember(Value = "payment")]
    Payment,
    /// <summary>
    /// Агентское вознаграждение
    /// </summary>
    [EnumMember(Value = "agent_commission")]
    AgentCommission,
    /// <summary>
    /// Составной предмет расчета
    /// </summary>
    [EnumMember(Value = "composite")]
    Composite,
    /// <summary>
    /// Иной предмет расчета
    /// </summary>
    [EnumMember(Value = "another")]
    Another,
}