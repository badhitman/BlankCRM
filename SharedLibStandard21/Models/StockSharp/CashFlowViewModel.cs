////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;
using System.ComponentModel.DataAnnotations;

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
    public DateTime PaymentDate { get; set; }
    
    /// <inheritdoc/>
    public decimal PaymentValue { get; set; }
    
    /// <inheritdoc/>
    public CashFlowTypesEnum CashFlowType { get; set; }
}