////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// Операция по счёту в Т-Банк
/// </summary>
public partial class TBankOperationBaseModel
{
    /// <inheritdoc/>
    [JsonPropertyName("operationDate")]
    public DateTimeOffset OperationDate { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("operationId")]
    public required string OperationId { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("operationStatus")]
    public required string OperationStatus { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("accountNumber")]
    public required string AccountNumber { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("bic")]
    public required string Bic { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("typeOfOperation")]
    public required string TypeOfOperation { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("trxnPostDate")]
    public DateTimeOffset TrxnPostDate { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("authorizationDate")]
    public DateTimeOffset AuthorizationDate { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("drawDate")]
    public DateTimeOffset DrawDate { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("chargeDate")]
    public DateTimeOffset ChargeDate { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("docDate")]
    public DateTimeOffset DocDate { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("documentNumber")]
    public required string DocumentNumber { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("payVo")]
    public required string PayVo { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("vo")]
    public required string Vo { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("priority")]
    public int Priority { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("operationAmount")]
    public decimal OperationAmount { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("operationCurrencyDigitalCode")]
    public required string OperationCurrencyDigitalCode { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("accountAmount")]
    public decimal AccountAmount { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("accountCurrencyDigitalCode")]
    public required string AccountCurrencyDigitalCode { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("rubleAmount")]
    public decimal RubleAmount { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("payPurpose")]
    public string? PayPurpose { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("authCode")]
    public string? AuthCode { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("rrn")]
    public string? Rrn { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("acquirerId")]
    public string? AcquirerId { get; set; }
}
