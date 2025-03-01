////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// Дом
/// </summary>
public class HouseKLADRModelDB : RootKLADRModelDB
{
    /// <inheritdoc/>
    public required string KORP { get; set; }

    /// <inheritdoc/>
    public static HouseKLADRModelDB Build(HouseKLADRModel x)
    {
        return new()
        {
            CODE = x.CODE,
            GNINMB = x.GNINMB,
            INDEX = x.INDEX,
            KORP = x.KORP,
            NAME = x.NAME,
            OCATD = x.OCATD,
            SOCR = x.SOCR,
            UNO = x.UNO,
        };
    }
}