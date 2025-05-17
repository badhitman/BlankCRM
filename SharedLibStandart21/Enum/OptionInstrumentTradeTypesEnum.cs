////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Option types.
/// </summary>
public enum OptionInstrumentTradeTypesEnum
{
    /// <inheritdoc/>
    [Description("None")]
    None,

    /// <summary>
    /// Call.
    /// </summary>
    Call,
    
    /// <summary>
    /// Put.
    /// </summary>
    Put
}