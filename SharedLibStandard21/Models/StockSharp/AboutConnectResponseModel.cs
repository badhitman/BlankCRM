////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// AboutConnectResponseModel
/// </summary>
public class AboutConnectResponseModel : ResponseBaseModel
{
    /// <summary>
    /// CanConnect
    /// </summary>
    public bool CanConnect { get; set; }

    /// <summary>
    /// ConnectionState
    /// </summary>
    public ConnectionStatesEnum? ConnectionState { get; set; }

    /// <summary>
    /// LastConnectedAt
    /// </summary>
    public DateTime? LastConnectedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public void Update(UpdateConnectionHandleModel req)
    {
        CanConnect = req.CanConnect;
        ConnectionState = req.ConnectionState;
        LastConnectedAt = DateTime.UtcNow;
    }
}