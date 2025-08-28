////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System;

namespace SharedLib;

/// <summary>
/// SFut
/// </summary>
public class SFut(string name, InstrumentTradeStockSharpModel sec, Dictionary<SBond, decimal> convfactors, decimal reporate, DateTime deliverydate)
{
    /// <inheritdoc/>
    public decimal ModelPrice { get; set; }

    /// <inheritdoc/>
    public InstrumentTradeStockSharpModel UnderlyingSecurity { get; set; } = sec;

    /// <inheritdoc/>
    public string Name { get; set; } = name;

    /// <inheritdoc/>
    public DateTime Deliverydate { get; set; } = deliverydate;

    /// <inheritdoc/>
    public decimal RepoRate { get; set; } = reporate;

    /// <inheritdoc/>
    public string? MicexCode { get; set; } = sec.Code;

    /// <inheritdoc/>
    public Dictionary<SBond, decimal> ConversionFactors { get; private set; } = convfactors;
}