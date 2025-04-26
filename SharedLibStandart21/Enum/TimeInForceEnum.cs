////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Limit order time in force.
/// </summary>
public enum TimeInForceEnum
{
    /// <summary>
    /// Good til cancelled.
    /// </summary>
    [Display( Name = "GTC", Description = "GoodTilCancelled")]
    PutInQueue,
    /// <summary>
    /// Fill Or Kill.
    /// </summary>
    [Display( Name = "FOK", Description = "FillOrKill")]
    MatchOrCancel,
    /// <summary>
    /// Immediate Or Cancel.
    /// </summary>
    [Display(Name = "IOC", Description = "ImmediateOrCancel")]
    CancelBalance
}
