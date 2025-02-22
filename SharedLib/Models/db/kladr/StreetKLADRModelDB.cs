////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public class StreetKLADRModelDB : RootKLADRModelDB
{
    /// <inheritdoc/>
    public static StreetKLADRModelDB Build(RootKLADRModel x)
    {
        return new()
        {
            CODE = x.CODE,
            GNINMB = x.GNINMB,
            INDEX = x.INDEX,
            NAME = x.NAME,
            OCATD = x.OCATD,
            SOCR = x.SOCR,
            UNO = x.UNO,
        };
    }
}