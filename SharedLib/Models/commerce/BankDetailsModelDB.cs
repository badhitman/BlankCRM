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
    public override string ToString()
        => $"{CurrentAccount} в {Name} (БИК:{BankBIC})";

    /// <inheritdoc/>
    public void Update(BankDetailsModelDB sender)
    {
        Organization = sender.Organization;
        OrganizationId = sender.OrganizationId;
        BankBIC = sender.BankBIC;
        CurrentAccount = sender.CurrentAccount;
        Id = sender.Id;
        IsDisabled = sender.IsDisabled;
        CorrespondentAccount = sender.CorrespondentAccount;
        Description = sender.Description;
        Name = sender.Name;
    }

    /// <inheritdoc/>
    public static BankDetailsModelDB BuildEmpty(OrganizationModelDB org)
        => new()
        {
            OrganizationId = org.Id,
            Organization = org,

            CorrespondentAccount = string.Empty,
            CurrentAccount = string.Empty,
            BankBIC = string.Empty,
            Name = string.Empty,
        };
}