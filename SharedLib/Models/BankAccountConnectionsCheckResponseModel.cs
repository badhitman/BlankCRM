////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// BankAccountConnectionsCheckResponseModel
/// </summary>
public class BankAccountConnectionsCheckResponseModel : ResponseBaseModel
{
    /// <inheritdoc/>

    public List<ConnectionAccount>? Accounts { get; set; }
}