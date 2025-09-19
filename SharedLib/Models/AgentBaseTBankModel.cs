////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// AgentBaseTBankModel
/// </summary>
public partial class AgentBaseTBankModel : AgentModel
{
    /// <inheritdoc/>
    [JsonPropertyName("bankName")]
    public required string BankName { get; set; }
}