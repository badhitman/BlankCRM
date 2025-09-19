////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// PayerTBankModel
/// </summary>
public partial class PayerTBankModel : AgentBaseTBankModel
{
    /// <inheritdoc/>
    [JsonPropertyName("acct")]
    public required string Acct { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("bicRu")]
    public required string BicRu { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("corAcct")]
    public required string CorAcct { get; set; }
}
