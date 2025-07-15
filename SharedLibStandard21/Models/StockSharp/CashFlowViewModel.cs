////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using System;

namespace SharedLib;

/// <summary>
/// CashFlowViewModel
/// </summary>
public class CashFlowViewModel
{
    /// <inheritdoc/>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public int InstrumentId { get; set; }

    /// <inheritdoc/>
    public DateTime PaymentDate { get; set; }

    /// <inheritdoc/>
    public decimal PaymentValue { get; set; }

    /// <summary>
    /// <see cref="CashFlowTypesEnum"/>
    /// </summary>
    public int CashFlowType { get; set; }

    /// <inheritdoc/>
    public void SetUpdate(CashFlowViewModel req)
    {
        Id = req.Id;
        PaymentDate = req.PaymentDate;
        InstrumentId = req.InstrumentId;
        PaymentValue = req.PaymentValue;
        CashFlowType = req.CashFlowType;
    }
}