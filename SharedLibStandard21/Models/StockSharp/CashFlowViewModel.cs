////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System;

namespace SharedLib;

/// <summary>
/// CashFlowViewModel
/// </summary>
public class CashFlowViewModel : IComparable<CashFlowViewModel>
{
    /// <inheritdoc/>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public int InstrumentId { get; set; }

    /// <inheritdoc/>
    public DateTime StartDate { get; set; }

    /// <inheritdoc/>
    public DateTime EndDate { get; set; }

    /// <inheritdoc/>
    public decimal CouponRate { get; set; }

    /// <inheritdoc/>
    public decimal Coupon { get; set; }

    /// <inheritdoc/>
    public decimal Notional { get; set; }

    /// <inheritdoc/>
    public int CompareTo(CashFlowViewModel other)
    {
        return EndDate.CompareTo(other.EndDate);
    }

    /// <inheritdoc/>
    public void SetUpdate(CashFlowViewModel req)
    {
        Id = req.Id;
        InstrumentId = req.InstrumentId;
        StartDate = req.StartDate;
        EndDate = req.EndDate;
        CouponRate = req.CouponRate;
        Coupon = req.Coupon;
        Notional = req.Notional;
    }
}