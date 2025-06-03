////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// MyTradeStockSharpModelDB
/// </summary>
public class MyTradeStockSharpViewModel : MyTradeStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    public int Id { get; set; }


    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }
}