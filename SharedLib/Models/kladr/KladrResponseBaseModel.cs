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
    public required KladrTypesObjectsEnum TypeObject { get; set; }

    /// <summary>
    /// Тип подчинения
    /// </summary>
    public required KladrChainTypesEnum ChainType { get; set; }

    /// <summary>
    /// Payload
    /// </summary>
    public required JObject Payload { get; set; }
}