////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Types messages
/// </summary>
public enum MessagesTypesEnum
{
    /// <summary>
    /// Error
    /// </summary>
    [Description("Error")]
    Error = -1,

    /// <summary>
    /// Success
    /// </summary>
    [Description("Success")]
    Success = 0,

    /// <summary>
    /// Info
    /// </summary>
    [Description("Info")]
    Info = 2,

    /// <summary>
    /// Warning
    /// </summary>
    [Description("Warning")]
    Warning = 3
}