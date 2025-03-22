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
    /// Расчетный счет
    /// </summary>
    [Required]
    public required string CurrentAccount { get; set; }

    /// <summary>
    /// Корр. счет
    /// </summary>
    [Required]
    public required string CorrespondentAccount { get; set; }

    /// <summary>
    /// Банк
    /// </summary>
    [Required]
    public required string BankName { get; set; }

    /// <summary>
    /// БИК Банка
    /// </summary>
    [Required]
    public required string BankBIC { get; set; }

    /// <inheritdoc/>
    public static BankDetailsModelDB BuildEmpty(int organizationId)
        => new()
        {
            BankBIC = string.Empty,
            BankName = string.Empty,
            CorrespondentAccount = string.Empty,
            CurrentAccount = string.Empty,
            Name = string.Empty,
            Description = string.Empty,
            OrganizationId = organizationId,
        };
}