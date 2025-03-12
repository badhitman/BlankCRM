////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// объект и все его предки
/// </summary>
public class KladrResponseBaseModel
{
    /// <summary>
    /// Тип объекта
    /// </summary>
    public KladrTypesObjectsEnum TypeObject { get; set; }

    /// <summary>
    /// Payload
    /// </summary>
    public required JObject Payload { get; set; }
}