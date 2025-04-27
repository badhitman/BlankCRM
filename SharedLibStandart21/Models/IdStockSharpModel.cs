////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InstrumentIdStockSharpModel
/// </summary>
public class IdStockSharpModel : IdStockSharpBaseModel
{
    /// <summary>
    /// Determines the id is StockSharp.Messages.SecurityId.Money or StockSharp.Messages.SecurityId.News.
    /// </summary>
    public bool IsSpecial { get; set; }

    /// <summary>
    /// Electronic board code.
    /// </summary>
    public string? BoardCode { get; set; }

    /// <summary>
    /// Security code.
    /// </summary>
    public string? SecurityCode { get; set; }
}