////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// PortfolioStatesEnum
/// </summary>
public enum PortfolioStatesEnum
{
    /// <summary>
    /// Active.
    /// </summary>
    [Display(Name = "Active")]
    Active,
    /// <summary>
    /// Blocked.
    /// </summary>
    [Display(Name = "Blocked")]
    Blocked
}
