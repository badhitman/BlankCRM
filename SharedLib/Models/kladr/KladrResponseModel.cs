////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// объект и все его предки
/// </summary>
public class KladrResponseModel : KladrBaseElementModel
{
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
    /// Почтовый индекс
    /// </summary>
    public required string PostIndex { get; set; }


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


    /// <inheritdoc/>
    public KladrResponseModel Build(string houseNum)
    {
        KladrResponseModel res = GlobalTools.CreateDeepCopy(this)!;
        res.Name = houseNum;
        return res;
    }


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
            : $"{string.Join(", ", Parents.Select(x => toString(x.NAME, x.SOCR, CodeKladrModel.Build(x.CODE))))}, {toString(Name, Socr, Metadata)}";
    }
}