////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// объект и все его предки
/// </summary>
public class KladrResponseModel : KladrResponseBaseModel
{
    /// <summary>
    /// Socrbase
    /// </summary>
    public required SocrbaseKLADRModelDB[] Socrbase { get; set; }

    /// <summary>
    /// Вышестоящие/предки
    /// </summary>
    public KeyValuePair<KladrTypesObjectsEnum, RootKLADRModelDB>[]? Parents { get; set; }
}