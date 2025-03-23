////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// BankDetailsModelDB
/// </summary>
[Index(nameof(BankBIC), nameof(CorrespondentAccount), nameof(CurrentAccount), IsUnique = true)]
public class BankDetailsModelDB : EntryDescriptionSwitchableModel
{
    /// <summary>
    /// Organization
    /// </summary>
    public OrganizationModelDB? Organization { get; set; }

    /// <summary>
    /// Organization
    /// </summary>
    public int OrganizationId { get; set; }

    /// <summary>
    /// БИК Банка
    /// </summary>
    [Required]
    public required string BankBIC { get; set; }

    /// <summary>
    /// Расчетный счет
    /// </summary>
    [Required]
    public required string CurrentAccount { get; set; }

    /// <summary>
    /// Корр. счет
    /// </summary>
    [Required]
    public required string CorrespondentAccount { get; set; }

    /// <inheritdoc/>
    public static BankDetailsModelDB BuildEmpty(OrganizationModelDB org)
        => new()
        {
            BankBIC = string.Empty,
            Name = string.Empty,
            CorrespondentAccount = string.Empty,
            CurrentAccount = string.Empty,
            OrganizationId = org.Id,
            Organization = org,
            Description = string.Empty,
            IsDisabled = false,
        };
}