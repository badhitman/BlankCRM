////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// CustomerBankId
/// </summary>
[Index(nameof(Name)), Index(nameof(Inn)), Index(nameof(BankIdentifyType))]
public class CustomerBankIdModelDB : CustomerBankIdBaseModel
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <summary>
    /// UserIdentityId
    /// </summary>
    [Required]
    public required string UserIdentityId { get; set; }
}