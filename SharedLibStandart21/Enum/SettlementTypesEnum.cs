////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Settlement types.
/// </summary>
public enum SettlementTypesEnum
{
    /// <inheritdoc/>
    [Description("None")]
    None,

    /// <summary>
    /// Delivery.
    /// </summary>
    Delivery,

    /// <summary>
    /// Cash.
    /// </summary>
    Cash
}