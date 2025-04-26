////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// InstrumentExternalIdModel
/// </summary>
public class InstrumentExternalIdModel
{
    /// <summary>
    /// Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// ID in SEDOL format (Stock Exchange Daily Official List).
    /// </summary>
    [Display(Name = "Sedol")]
    public string? Sedol { get; set; }

    /// <summary>
    /// ID in CUSIP format (Committee on Uniform Securities Identification Procedures).
    /// </summary>
    [Display(Name = "Cusip")]
    public string? Cusip { get; set; }

    /// <summary>
    /// ID in ISIN format (International Securities Identification Number).
    /// </summary>
    [Display(Name = "Isin")]
    public string? Isin { get; set; }

    /// <summary>
    /// ID in RIC format (Reuters Instrument Code).
    /// </summary>
    [Display(Name = "Ric")]
    public string? Ric { get; set; }

    /// <summary>
    /// ID in Bloomberg format.
    /// </summary>
    [Display(Name = "Bloomberg")]
    public string? Bloomberg { get; set; }

    /// <summary>
    /// ID in IQFeed format.
    /// </summary>
    [Display(Name = "IQFeed")]
    public string? IQFeed { get; set; }

    /// <summary>
    /// ID in Interactive Brokers format.
    /// </summary>
    [Display(Name = "InteractiveBrokers")]
    public int? InteractiveBrokers { get; set; }

    /// <summary>
    /// ID in Plaza format.
    /// </summary>
    [Display(Name = "Plaza")]
    public string? Plaza { get; set; }

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
    public override bool Equals(object? other)
    {
        if (other is null)
            return false;

        return Equals((InstrumentExternalIdModel)other);
    }

    /// <inheritdoc/>
    public bool Equals(InstrumentExternalIdModel other)
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
    public static bool operator !=(InstrumentExternalIdModel left, InstrumentExternalIdModel right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public static bool operator ==(InstrumentExternalIdModel left, InstrumentExternalIdModel right)
    {
        return left?.Equals(right) ?? false;
    }
}