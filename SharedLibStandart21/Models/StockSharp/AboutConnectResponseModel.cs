////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AboutConnectResponseModel
/// </summary>
public class AboutConnectResponseModel:ResponseBaseModel
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