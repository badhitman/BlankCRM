////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SharedLib;

/// <summary>
/// Языки платежной формы
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum LanguageFormTBankEnum
{
    /// <summary>
    /// 
    /// </summary>
    [EnumMember(Value = "en")]
    En = 1,
    
    /// <summary>
    /// 
    /// </summary>
    [EnumMember(Value = "ru")]
    Ru,
}