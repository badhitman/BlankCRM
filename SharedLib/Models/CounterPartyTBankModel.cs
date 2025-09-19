////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// Контрагент
/// </summary>
public partial class CounterPartyTBankModel : AgentBaseTBankModel
{
    /// <inheritdoc/>
    [JsonPropertyName("account")]
    public required string Account { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("bankBic")]
    public required string BankBic { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("corrAccount")]
    public required string CorrAccount { get; set; }
}
