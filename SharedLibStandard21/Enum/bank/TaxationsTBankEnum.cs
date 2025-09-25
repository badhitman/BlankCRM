////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// Система налогообложения
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TaxationsTBankEnum
{
    /// <summary>
    /// Общая
    /// </summary>
    [EnumMember(Value = "osn")]
    Osn = 1,

    /// <summary>
    /// Упрощенная (доходы)
    /// </summary>
    [EnumMember(Value = "usn_income")]
    UsnIncome,

    /// <summary>
    /// Упрощенная (доходы минус расходы)
    /// </summary>
    [EnumMember(Value = "usn_income_outcome")]
    UsnIncomeOutcome,

    /// <summary>
    /// Патентная
    /// </summary>
    [EnumMember(Value = "patent")]
    Patent,

    /// <summary>
    /// Единый налог на вмененный доход
    /// </summary>
    [EnumMember(Value = "envd")]
    Envd,

    /// <summary>
    /// Единый сельскохозяйственный налог
    /// </summary>
    [EnumMember(Value = "esn")]
    Esn,
}