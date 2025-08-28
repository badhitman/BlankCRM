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
    public virtual string? Code { get; set; }

    /// <summary>
    /// Exchange
    /// </summary>
    public virtual ExchangeStockSharpModel? Exchange { get; set; }

    /// <summary>
    /// Exchange
    /// </summary>
    public virtual ExchangeStockSharpModel? GetExchange() => Exchange;

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (obj is BoardStockSharpModel other)
        {
            if (Code != other.Code)
                return false;

            if (Exchange is null && other.Exchange is null)
                return true;

            if (Exchange is null || other.Exchange is null)
                return false;

            return Exchange.Equals(other.Exchange);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(BoardStockSharpModel other)
    {
        if (other is null || Code != other.Code)
            return false;

        ExchangeStockSharpModel? _oe = GetExchange();

        if (_oe is null && other.Exchange is null)
            return true;

        if (_oe is null || other.Exchange is null)
            return false;

        return _oe.Equals(other.Exchange);
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

        if (left is null || right is null || left.Code != right.Code)
            return false;

        if (left.Exchange is null && right.Exchange is null)
            return true;

        if (left.Exchange is null || right.Exchange is null)
            return false;

        return left.Exchange.Equals(right.Exchange);
    }

    /// <inheritdoc/>
    public static bool operator !=(BoardStockSharpModel left, BoardStockSharpModel right)
    {
        if (left is null && right is null)
            return false;

        if (left is null || right is null || left.Code != right.Code)
            return true;


        if (left.Exchange is null && right.Exchange is null)
            return false;

        if (left.Exchange is null || right.Exchange is null)
            return true;

        return !left.Exchange.Equals(right.Exchange);
    }
}