////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// BoardStockSharpModel
/// </summary>
public class BoardStockSharpModel
{
    /// <summary>
    /// Code
    /// </summary>
    public virtual string? Code { get; set; }

    /// <summary>
    /// Exchange
    /// </summary>
    public virtual ExchangeStockSharpModel? Exchange { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Code} ({Exchange})";
    }
}