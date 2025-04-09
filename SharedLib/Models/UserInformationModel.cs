////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Информация о пользователе
/// </summary>
public class UserInformationModel
{
    /// <inheritdoc/>
    public required string Login { get; set; }

    /// <inheritdoc/>
    public required string FirstName { get; set; }

    /// <inheritdoc/>
    public string? Email { get; set; }

    /// <inheritdoc/>
    public string? LastName { get; set; }

    /// <inheritdoc/>
    public string? PatronymicName { get; set; }

    /// <inheritdoc/>
    public string? Description { get; set; }
}