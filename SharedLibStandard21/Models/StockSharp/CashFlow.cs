////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// CashFlow
/// </summary>
public class CashFlow(DateTime stdate, DateTime enddate, decimal coupon, decimal notional, decimal couprate) : IComparable<CashFlow>
{
    /// <inheritdoc/>
    public DateTime StDate = stdate;
    /// <inheritdoc/>
    public DateTime EndDate = enddate;
    /// <inheritdoc/>
    public decimal CouponRate = couprate;
    /// <inheritdoc/>
    public decimal Coupon = coupon;
    /// <inheritdoc/>
    public decimal Notional = notional;

    /// <inheritdoc/>
    public int CompareTo(CashFlow other)
    {
        return EndDate.CompareTo(other.EndDate);
    }
}