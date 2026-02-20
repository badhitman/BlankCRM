////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// CustomerBankIdBaseModel
/// </summary>
public class CustomerBankIdBaseModel
{
    /// <inheritdoc/>
    public string? Name { get; set; }

    /// <inheritdoc/>
    public string? Inn { get; set; }

    /// <summary>
    /// IdentifyType
    /// </summary>
    public BanksIdentifyTypesEnum BankIdentifyType { get; set; }
}