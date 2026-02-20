////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SharedLib;

/// <summary>
/// Типы возвращаемых данных QR СБП/НСПК
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum DataTypeQREnum
{
    /// <summary>
    /// Payload
    /// </summary>
    [EnumMember(Value = "PAYLOAD")]
    PAYLOAD,
    /// <summary>
    /// Image SVG
    /// </summary>
    [EnumMember(Value = "IMAGE")]
    IMAGE,
}