////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Подключение к банку
/// </summary>
public class BankConnectionBaseModel
{
    /// <inheritdoc/>
    public string? Token { get; set; }

    /// <inheritdoc/>
    public required string Name { get; set; }

    /// <inheritdoc/>
    public BankInterfacesEnum BankInterface { get; set; }

    /// <summary>
    /// The date and time when the address was last checked.
    /// </summary>
    public DateTime? LastChecked { get; set; }
}
