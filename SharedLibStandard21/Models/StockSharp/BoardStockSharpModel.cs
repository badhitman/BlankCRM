////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// BoardStockSharpModel
/// </summary>
public class BoardStockSharpModel : IEquatable<BoardStockSharpModel>
{
    /// <summary>
    /// Code
    /// </summary>
    public virtual string Code { get; set; }

    /// <summary>
    /// Exchange
    /// </summary>
    public virtual ExchangeStockSharpModel Exchange { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is not null && obj is BoardStockSharpModel other)
            return Code == other.Code && Exchange?.Equals(other.Exchange) == true;

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(BoardStockSharpModel other)
    {
        if (other is null)
            return false;

        return Code == other.Code && Exchange.Equals(other.Exchange);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Exchange);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Code} ({Exchange})";
    }

    /// <inheritdoc/>
    public static bool operator ==(BoardStockSharpModel left, BoardStockSharpModel right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Code == right.Code && left.Exchange == right.Exchange;
    }

    /// <inheritdoc/>
    public static bool operator !=(BoardStockSharpModel left, BoardStockSharpModel right)
    {
        if (left is null && right is null)
            return false;

        if (left is null || right is null)
            return true;

        return left.Code != right.Code || left.Exchange != right.Exchange;
    }
}