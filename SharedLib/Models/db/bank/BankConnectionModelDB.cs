////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Подключение к банку
/// </summary>
[Index(nameof(BankInterface)), Index(nameof(LastChecked))]
public class BankConnectionModelDB : BankConnectionBaseModel
{
    /// <inheritdoc/>
    public int Id { get; set; }
}