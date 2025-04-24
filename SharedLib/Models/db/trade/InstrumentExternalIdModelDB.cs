////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// ID in other systems.
/// </summary>
public class InstrumentExternalIdModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    ///<inheritdoc/>
    public InstrumentTradeModelDB? ParentInstrument { get; set; }
    ///<inheritdoc/>
    public int ParentInstrumentId { get; set; }

    /// <summary>
    /// ID in SEDOL format (Stock Exchange Daily Official List).
    /// </summary>
    [Display(Name = "Sedol")]
    public required string Sedol
    {
        get; set;
    }

    /// <summary>
    /// ID in CUSIP format (Committee on Uniform Securities Identification Procedures).
    /// </summary>
    [Display(Name = "Cusip")]
    public required string Cusip
    {
        get; set;
    }

    /// <summary>
    /// ID in ISIN format (International Securities Identification Number).
    /// </summary>
    [Display(Name = "Isin")]
    public required string Isin
    {
        get; set;
    }

    /// <summary>
    /// ID in RIC format (Reuters Instrument Code).
    /// </summary>
    [Display(Name = "Ric")]
    public required string Ric
    {
        get; set;
    }

    /// <summary>
    /// ID in Bloomberg format.
    /// </summary>
    [Display(Name = "Bloomberg")]
    public required string Bloomberg
    {
        get; set;
    }

    /// <summary>
    /// ID in IQFeed format.
    /// </summary>
    [Display(Name = "IQFeed")]
    public required string IQFeed
    {
        get; set;
    }

    /// <summary>
    /// ID in Interactive Brokers format.
    /// </summary>
    [Display(Name = "InteractiveBrokers")]
    public int? InteractiveBrokers
    {
        get; set;
    }

    /// <summary>
    /// ID in Plaza format.
    /// </summary>
    [Display(Name = "Plaza")]
    public required string Plaza
    {
        get; set;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        string text = string.Empty;
        if (!Bloomberg.IsEmpty())
        {
            text = text + " Bloom " + Bloomberg;
        }

        if (!Cusip.IsEmpty())
        {
            text = text + " CUSIP " + Cusip;
        }

        if (!IQFeed.IsEmpty())
        {
            text = text + " IQFeed " + IQFeed;
        }

        if (!Isin.IsEmpty())
        {
            text = text + " ISIN " + Isin;
        }

        if (!Ric.IsEmpty())
        {
            text = text + " RIC " + Ric;
        }

        if (!Sedol.IsEmpty())
        {
            text = text + " SEDOL " + Sedol;
        }

        if (InteractiveBrokers.HasValue)
        {
            text += $" InteractiveBrokers {InteractiveBrokers}";
        }

        if (!Plaza.IsEmpty())
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
    public override bool Equals(object? other)
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