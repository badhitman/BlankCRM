////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectWalletsRetailsRequestModel
/// </summary>
public class SelectWalletsRetailsRequestModel
{
    /// <summary>
    /// UserIdentityId
    /// </summary>
    public string[]? UsersFilterIdentityId { get; set; }

    /// <summary>
    /// Автоматически создавать недостающие кошельки
    /// </summary>
    public bool AutoGenerationWallets { get; set; }
}