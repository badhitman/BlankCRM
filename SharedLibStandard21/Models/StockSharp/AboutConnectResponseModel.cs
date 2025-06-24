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
    /// <inheritdoc/>
    public bool CanConnect { get; set; }

    /// <inheritdoc/>
    public ConnectionStatesEnum? ConnectionState { get; set; }

    /// <inheritdoc/>
    public DateTime? LastConnectedAt { get; set; }

    /// <inheritdoc/>
    public bool StrategyStarted { get; set; }

    /// <inheritdoc/>
    public decimal LowLimit { get; set; }

    /// <inheritdoc/>
    public decimal HighLimit { get; set; }

    /// <inheritdoc/>
    public string SecurityCriteriaCodeFilterStockSharp { get; set; }

    /// <inheritdoc/>
    public void Update(UpdateConnectionHandleModel req)
    {
        CanConnect = req.CanConnect;
        ConnectionState = req.ConnectionState;
        LastConnectedAt = DateTime.UtcNow;
    }

    /// <inheritdoc/>
    public void Update(AboutConnectResponseModel req)
    {
        CanConnect = req.CanConnect;
        ConnectionState = req.ConnectionState;
        LastConnectedAt = req.LastConnectedAt;
        StrategyStarted = req.StrategyStarted;
        LowLimit = req.LowLimit;
        HighLimit = req.HighLimit;
        SecurityCriteriaCodeFilterStockSharp = req.SecurityCriteriaCodeFilterStockSharp;
        Messages = req.Messages;
    }
}