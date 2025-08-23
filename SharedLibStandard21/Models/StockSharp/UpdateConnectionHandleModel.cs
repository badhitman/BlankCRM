////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// UpdateConnectionHandleModel
/// </summary>
public class UpdateConnectionHandleModel
{
    /// <summary>
    /// CanConnect
    /// </summary>
    public bool CanConnect { get; set; }

    /// <summary>
    /// ConnectionState
    /// </summary>
    public ConnectionStatesEnum? ConnectionState { get; set; }
}