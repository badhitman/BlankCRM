////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// CashFlowStockSharpRequestModel
/// </summary>
public class CashFlowStockSharpRequestModel
{
    /// <inheritdoc/>
    public int FromDays { get; set; }

    /// <inheritdoc/>
    public decimal NotionalFirst { get; set; }

    /// <inheritdoc/>
    public int InstrumentId { get; set; }

    /// <inheritdoc./>
    public DateTime? IssueDate { get; set; }

    /// <inheritdoc./>
    public DateTime? MaturityDate { get; set; }
}