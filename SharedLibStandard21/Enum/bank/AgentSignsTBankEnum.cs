////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SharedLib;

/// <summary>
/// Признак агента
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum AgentSignsTBankEnum
{
    /// <summary>
    /// Банковский платежный агент
    /// </summary>
    [EnumMember(Value = "bank_paying_agent")]
    BankPayingAgent = 1,
    /// <summary>
    /// Банковский платежный субагент
    /// </summary>
    [EnumMember(Value = "bank_paying_subagent")]
    BankPayingSubagent,
    /// <summary>
    /// Платежный агент
    /// </summary>
    [EnumMember(Value = "paying_agent")]
    PayingAgent,
    /// <summary>
    /// Платежный субагент
    /// </summary>
    [EnumMember(Value = "paying_subagent")]
    PayingSubagent,
    /// <summary>
    /// Поверенный
    /// </summary>
    [EnumMember(Value = "attorney")]
    Attorney,
    /// <summary>
    /// Комиссионер
    /// </summary>
    [EnumMember(Value = "commission_agent")]
    CommissionAgent,
    /// <summary>
    /// Другой тип агента
    /// </summary>
    [EnumMember(Value = "another")]
    Another,
}