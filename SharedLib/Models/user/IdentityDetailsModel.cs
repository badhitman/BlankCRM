////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// IdentityDetailsModel
/// </summary>
public class IdentityDetailsModel
{
    /// <summary>
    /// UserId
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// FirstName
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// LastName
    /// </summary>
    public string? LastName { get; set; }

    /// <inheritdoc/>
    public string? Patronymic { get; set; }

    /// <summary>
    /// PhoneNum
    /// </summary>
    public string? PhoneNum { get; set; }

    /// <summary>
    /// Идентификатор (внешний)
    /// </summary>
    public string? ExternalUserId { get; set; }

    #region address
    /// <inheritdoc/>
    [Required]
    public string? KladrCode { get; set; }

    /// <inheritdoc/>
    [Required]
    public string? KladrTitle { get; set; }

    /// <summary>
    /// Адрес 
    /// </summary>
    [Required]
    public string? AddressUserComment { get; set; }
    #endregion


    /// <inheritdoc/>
    public bool UpdateAddress { get; set; }
}