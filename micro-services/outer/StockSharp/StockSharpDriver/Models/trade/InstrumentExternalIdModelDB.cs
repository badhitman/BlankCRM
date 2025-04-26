////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// ID in other systems.
/// </summary>
[Index(nameof(LastAtUpdatedUTC)), Index(nameof(Plaza)), Index(nameof(InteractiveBrokers)), Index(nameof(IQFeed)), Index(nameof(Bloomberg)), Index(nameof(Ric)), Index(nameof(Isin)), Index(nameof(Cusip)), Index(nameof(Sedol))]
public class InstrumentExternalIdModelDB : InstrumentIdStockSharpBaseModel, IBaseStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    ///<inheritdoc/>
    public InstrumentTradeModelDB ParentInstrument { get; set; }
    ///<inheritdoc/>
    public int ParentInstrumentId { get; set; }

    /// <inheritdoc/>
    public DateTime LastAtUpdatedUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        string text = string.Empty;
        if (!string.IsNullOrWhiteSpace(Bloomberg))
        {
            text = text + " Bloom " + Bloomberg;
        }

        if (!string.IsNullOrWhiteSpace(Cusip))
        {
            text = text + " CUSIP " + Cusip;
        }

        if (!string.IsNullOrWhiteSpace(IQFeed))
        {
            text = text + " IQFeed " + IQFeed;
        }

        if (!string.IsNullOrWhiteSpace(Isin))
        {
            text = text + " ISIN " + Isin;
        }

        if (!string.IsNullOrWhiteSpace(Ric))
        {
            text = text + " RIC " + Ric;
        }

        if (!string.IsNullOrWhiteSpace(Sedol))
        {
            text = text + " SEDOL " + Sedol;
        }

        if (InteractiveBrokers.HasValue)
        {
            text += $" InteractiveBrokers {InteractiveBrokers}";
        }

        if (!string.IsNullOrWhiteSpace(Plaza))
        {
            text = text + " Plaza " + Plaza;
        }

        return text;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object other)
    {
        if (other is null)
            return false;

        return Equals((InstrumentExternalIdModelDB)other);
    }

    /// <inheritdoc/>
    public bool Equals(InstrumentExternalIdModelDB other)
    {
        if (other is null)
        {
            return false;
        }

        if (Bloomberg != other.Bloomberg)
        {
            return false;
        }

        if (Cusip != other.Cusip)
        {
            return false;
        }

        if (IQFeed != other.IQFeed)
        {
            return false;
        }

        if (Isin != other.Isin)
        {
            return false;
        }

        if (Ric != other.Ric)
        {
            return false;
        }

        if (Sedol != other.Sedol)
        {
            return false;
        }

        if (InteractiveBrokers != other.InteractiveBrokers)
        {
            return false;
        }

        if (Plaza != other.Plaza)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public static bool operator !=(InstrumentExternalIdModelDB left, InstrumentExternalIdModelDB right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public static bool operator ==(InstrumentExternalIdModelDB left, InstrumentExternalIdModelDB right)
    {
        return left?.Equals(right) ?? false;
    }
}