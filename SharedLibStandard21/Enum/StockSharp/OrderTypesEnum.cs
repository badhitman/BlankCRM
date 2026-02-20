////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Order types.
/// </summary>
public enum OrderTypesEnum
{
    /// <summary>
    /// Limit.
    /// </summary>
    [Description("Limit.")]
    Limit,

    /// <summary>
    /// Market.
    /// </summary>
    [Description("Market.")]
    Market,

    /// <summary>
    /// Conditional (stop-loss, take-profit).
    /// </summary>
    [Description("Conditional (stop-loss, take-profit).")]
    Conditional
}
