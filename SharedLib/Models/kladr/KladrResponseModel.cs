////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// объект и все его предки
/// </summary>
public class KladrResponseModel
{
    /// <summary>
    /// Socrbase
    /// </summary>
    public required SocrbaseKLADRModelDB[] Socrbases { get; set; }

    /// <summary>
    /// Вышестоящие/предки
    /// </summary>
    public List<RootKLADRModelDB>? Parents { get; set; }

    /// <summary>
    /// Payload
    /// </summary>
    public required JObject Payload { get; set; }
}