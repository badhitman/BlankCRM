////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// ObjectStatesEnum
/// </summary>
public enum ObjectStatesEnum
{
    /// <summary>
    /// Default
    /// </summary>
    [Description("Default")]
    Default = 0,

    /// <summary>
    /// IsFavorite
    /// </summary>
    [Description("IsFavorite")]
    IsFavorite = 10,

    /// <summary>
    /// IsDisabled
    /// </summary>
    [Description("IsDisabled")]
    IsDisabled = 20,
}