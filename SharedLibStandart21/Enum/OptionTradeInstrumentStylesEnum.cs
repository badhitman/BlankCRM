////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// OptionTradeInstrumentStylesEnum
/// </summary>
public enum OptionTradeInstrumentStylesEnum
{
    /// <inheritdoc/>
    [Description("None")]
    None,

    /// <summary>
    /// European
    /// </summary>
    European,

    /// <summary>
    /// American.
    /// </summary>
    American,

    /// <summary>
    /// Exotic.
    /// </summary>
    Exotic
}