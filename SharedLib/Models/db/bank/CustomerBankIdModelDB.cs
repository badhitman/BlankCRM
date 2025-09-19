////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// CustomerBankId
/// </summary>
[Index(nameof(Name)), Index(nameof(Inn)), Index(nameof(BankIdentifyType))]
public class CustomerBankIdModelDB : CustomerBankIdBaseModel
{
    /// <inheritdoc/>
    public long Id { get; set; }
}