////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ExchangeStockSharpModel
/// </summary>
public class ExchangeStockSharpModel
{
    /// <summary>
    /// Name
    /// </summary>
    public virtual string? Name { get; set; }

    /// <summary>
    /// CountryCode
    /// </summary>
    public virtual CountryCodesEnum CountryCode { get; set; }
}