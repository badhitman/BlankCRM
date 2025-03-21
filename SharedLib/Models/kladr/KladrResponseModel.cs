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
    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} {Socr}";
    }

    /// <summary>
    /// Code
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// Socr
    /// </summary>
    public required string Socr { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public required string Name { get; set; }


    /// <summary>
    /// UNO
    /// </summary>
    public required string UNO { get; set; }

    /// <summary>
    /// OCATD
    /// </summary>
    public required string OCATD { get; set; }

    /// <summary>
    /// GNINMB
    /// </summary>
    public required string GNINMB { get; set; }


    /// <summary>
    /// Chain
    /// </summary>
    public required KladrChainTypesEnum Chain {  get; set; }

    /// <summary>
    /// Payload
    /// </summary>
    public required JObject Payload { get; set; }

    /// <summary>
    /// Socrbase
    /// </summary>
    public required SocrbaseKLADRModelDB[] Socrbases { get; set; }

    /// <summary>
    /// Вышестоящие/предки
    /// </summary>
    public List<RootKLADRModelDB>? Parents { get; set; }
}