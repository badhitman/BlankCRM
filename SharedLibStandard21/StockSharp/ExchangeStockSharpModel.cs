////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// ExchangeStockSharpModel
/// </summary>
public class ExchangeStockSharpModel : IEquatable<ExchangeStockSharpModel>
{
    /// <summary>
    /// Name
    /// </summary>
    public virtual string? Name { get; set; }

    /// <summary>
    /// CountryCode
    /// </summary>
    public virtual int? CountryCode { get; set; }


    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is ExchangeStockSharpModel other)
            return other is not null && Name == other.Name && CountryCode == other.CountryCode;

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(ExchangeStockSharpModel obj)
    {
        if (obj is null)
            return false;

        if (obj is ExchangeStockSharpModel other)
            return other is not null && Name == other.Name && CountryCode == other.CountryCode;

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, CountryCode);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name}{(CountryCode is null || CountryCode == 0 ? "" : $" - {(CountryCodesEnum)CountryCode}")}";
    }

    /// <inheritdoc/>
    public static bool operator ==(ExchangeStockSharpModel left, ExchangeStockSharpModel right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Name == right.Name && left.CountryCode == right.CountryCode;
    }

    /// <inheritdoc/>
    public static bool operator !=(ExchangeStockSharpModel left, ExchangeStockSharpModel right)
    {
        if (left is null && right is null)
            return false;

        if (left is null || right is null)
            return true;

        return left.Name != right.Name || left.CountryCode != right.CountryCode;
    }
}