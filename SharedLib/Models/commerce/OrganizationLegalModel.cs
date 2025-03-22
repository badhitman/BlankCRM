////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// OrganizationLegalModel
/// </summary>
[Index(nameof(INN), IsUnique = true), Index(nameof(OGRN), IsUnique = true)]
public class OrganizationLegalModel : EntryDescriptionSwitchableModel
{
    /// <summary>
    /// Телефон
    /// </summary>
    [Required]
    public required string Phone { get; set; }

    /// <summary>
    /// Телефон
    /// </summary>
    [Required]
    public required string Email { get; set; }


    /// <summary>
    /// Юридический адрес
    /// </summary>
    [Required]
    public required string LegalAddress { get; set; }

    /// <summary>
    /// ИНН
    /// </summary>
    [Required]
    public required string INN { get; set; }

    /// <summary>
    /// КПП
    /// </summary>
    public string? KPP { get; set; }

    /// <summary>
    /// ОГРН
    /// </summary>
    [Required]
    public required string OGRN { get; set; }

    /// <summary>
    /// Банковские реквизиты (основные)
    /// </summary>
    public int BankMainAccount { get; set; }


    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} (ИНН:{INN})";
    }
}