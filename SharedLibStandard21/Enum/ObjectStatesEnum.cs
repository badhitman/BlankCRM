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
    Default = 10,

    /// <summary>
    /// IsFavorite
    /// </summary>
    [Description("IsFavorite")]
    IsFavorite = 20,

    /// <summary>
    /// IsDisabled
    /// </summary>
    [Description("IsDisabled")]
    IsDisabled = 30,
}