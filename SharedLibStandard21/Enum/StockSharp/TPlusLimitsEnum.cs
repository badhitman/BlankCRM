////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Т+ limit types.
/// </summary>
public enum TPlusLimitsEnum
{
    /// <summary>
    /// Т+0.
    /// </summary>
    [Display(Name = "T+0")]
    T0,

    /// <summary>
    /// Т+1.
    /// </summary>
    [Display(Name = "T+1")]
    T1,

    /// <summary>
    /// Т+2.
    /// </summary>
    [Display(Name = "T+2")]
    T2,

    /// <summary>
    /// Т+x.
    /// </summary>
    [Display(Name = "T+x")]
    Tx
}