////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// OrderStatesEnum
/// </summary>
public enum OrderStatesEnum
{
    /// <summary>Not sent to the trading system.
    /// 
    /// </summary>
    /// <remarks>
    /// The original state of the order, when the transaction is not sent to the trading system.
    /// </remarks>
    None,
    /// <summary>
    /// The order is accepted by the exchange and is active.
    /// </summary>
    Active,
    /// <summary>
    /// The order is no longer active on an exchange (it was fully matched or cancelled).
    /// </summary>
    Done,
    /// <summary>
    /// The order is not accepted by the trading system.
    /// </summary>
    Failed,
    /// <summary>
    /// Pending registration.
    /// </summary>
    Pending
}
