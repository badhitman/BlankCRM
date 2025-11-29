////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectBanksRequestModel
/// </summary>
public class SelectConnectionsBanksRequestModel
{
    /// <summary>
    /// Get only active connections: those that have activated accounts.
    /// </summary>
    public bool? FilterOfEnabled { get; set; }
}