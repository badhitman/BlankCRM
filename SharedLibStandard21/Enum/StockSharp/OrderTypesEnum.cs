////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Order types.
/// </summary>
public enum OrderTypesEnum
{
    /// <summary>
    /// Limit.
    /// </summary>
    Limit,

    /// <summary>
    /// Market.
    /// </summary>
    Market,

    /// <summary>
    /// Conditional (stop-loss, take-profit).
    /// </summary>
    Conditional
}
