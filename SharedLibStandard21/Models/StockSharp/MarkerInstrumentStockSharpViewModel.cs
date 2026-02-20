////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// MarkerInstrumentStockSharpViewModel
/// </summary>
public class MarkerInstrumentStockSharpViewModel : IEquatable<MarkerInstrumentStockSharpViewModel>
{
    /// <inheritdoc/>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public MarkersInstrumentStockSharpEnum MarkerDescriptor { get; set; }

    /// <inheritdoc/>
    public bool Equals(MarkerInstrumentStockSharpViewModel other)
    {
        if (other is null)
            return false;

        return other.Id == Id && other.MarkerDescriptor == MarkerDescriptor;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (obj is MarkerInstrumentStockSharpViewModel other)
            return other.Id == Id && other.MarkerDescriptor == MarkerDescriptor;

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, MarkerDescriptor);
    }
}