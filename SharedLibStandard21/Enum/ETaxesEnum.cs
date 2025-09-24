////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SharedLib;

/// <summary>
/// Ставка НДС
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum ETaxesEnum
{
    /// <summary>
    /// Без НДС
    /// </summary>
    [EnumMember(Value = "none")]
    None = 1,

    /// <summary>
    /// 0%
    /// </summary>
    [EnumMember(Value = "vat0")]
    Vat0,

    /// <summary>
    /// 10%
    /// </summary>
    [EnumMember(Value = "vat10")]
    Vat10,

    /// <summary>
    /// 20%
    /// </summary>
    [EnumMember(Value = "vat20")]
    Vat20,

    /// <summary>
    /// 10/110
    /// </summary>
    [EnumMember(Value = "vat110")]
    Vat110,

    /// <summary>
    /// 20/120
    /// </summary>
    [EnumMember(Value = "vat120")]
    Vat120,
}