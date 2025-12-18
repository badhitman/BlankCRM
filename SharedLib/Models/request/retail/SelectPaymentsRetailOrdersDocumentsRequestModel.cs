////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// SelectPaymentsRetailOrdersDocumentsRequestModel
/// </summary>
public class SelectPaymentsRetailOrdersDocumentsRequestModel
{
    /// <summary>
    /// UserIdentityId
    /// </summary>
    public string? PayerFilterIdentityId { get; set; }

    /// <inheritdoc/>
    public PaymentsRetailTypesEnum[]? TypesFilter { get; set; }

    /// <inheritdoc/>
    public PaymentsRetailStatusesEnum[]? StatusesFilter { get; set; }

    /// <inheritdoc/>
    public DateTime? Start { get; set; }

    /// <inheritdoc/>
    public DateTime? End { get; set; }

    /// <summary>
    /// Исключить из выдачи платежи, которые уже связаны с указанным заказом
    /// </summary>
    public int ExcludeOrderId { get; set; }
}