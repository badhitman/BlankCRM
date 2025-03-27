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
    public CodeKladrModel Metadata => CodeKladrModel.Build(Code);


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


    /// <summary>
    /// Full name
    /// </summary>
    /// <returns></returns>
    public string GetFullName()
    {
        static string toString(string name, string socr, CodeKladrModel md) => socr[..1].Equals(socr[..1].ToUpper()) || md.Level >= KladrTypesObjectsEnum.City
            ? $"{socr} {name}"
            : $"{name} {socr}";

        return Parents is null || Parents.Count == 0
            ? toString(Name, Socr, Metadata)
            : $"{string.Join(", ", Parents.Select(x => toString(x.NAME, x.SOCR, Metadata)))}, {toString(Name, Socr, Metadata)}";
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} {Socr}";
    }
}