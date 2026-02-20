////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// Операция по счёту в Т-Банк
/// </summary>
public partial class TBankOperationModel : TBankOperationBaseModel
{
    /// <inheritdoc/>
    [JsonPropertyName("payer")]
    public PayerTBankModel? Payer { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("receiver")]
    public PayerTBankModel? Receiver { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("counterParty")]
    public CounterPartyTBankModel? CounterParty { get; set; }
}
