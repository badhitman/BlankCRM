////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// UserInfoBaseModel
/// </summary>
public record UserInfoBaseModel
{
    /// <inheritdoc/>
    public required string UserName { get; set; }

    /// <summary>
    /// FirstName
    /// </summary>
    public string? GivenName { get; set; }

    /// <summary>
    /// LastName
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// Patronymic
    /// </summary>
    public string? Patronymic { get; set; }

    /// <inheritdoc/>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Идентификатор (внешний)
    /// </summary>
    public string? ExternalUserId { get; set; }
}