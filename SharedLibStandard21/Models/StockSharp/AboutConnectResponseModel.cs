////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System;

namespace SharedLib;

/*
 string?
        ProgramDataPath,
        ClientCodeStockSharp,
        SecurityCriteriaCodeFilter,
        BoardCriteriaCodeFilter;
 */

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
    public string? SecurityCriteriaCodeFilter { get; set; }

    /// <inheritdoc/>
    public string? ClientCode { get; set; }

    /// <inheritdoc/>
    public string? ProgramPath { get; set; }

    /// <inheritdoc/>
    public CurveBaseModel? Curve { get; set; }

    /// <inheritdoc/>
    public void Update(UpdateConnectionHandleModel req)
    {
        CanConnect = req.CanConnect;
        ConnectionState = req.ConnectionState;
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
        SecurityCriteriaCodeFilter = req.SecurityCriteriaCodeFilter;
        ClientCode = req.ClientCode;
        ProgramPath = req.ProgramPath;
        Messages = req.Messages;
        Curve = req.Curve;
    }
}