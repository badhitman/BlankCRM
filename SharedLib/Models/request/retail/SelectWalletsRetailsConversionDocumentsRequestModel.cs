////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// SelectWalletsRetailsConversionDocumentsRequestModel
/// </summary>
public class SelectWalletsRetailsConversionDocumentsRequestModel
{
    /// <inheritdoc/>
    public string[]? SendersUserFilter { get; set; }

    /// <inheritdoc/>
    public string[]? RecipientsUserFilter { get; set; }

    /// <inheritdoc/>
    public bool IncludeDisabled { get; set; }

    /// <inheritdoc/>
    public DateTime? Start { get; set; }

    /// <inheritdoc/>
    public DateTime? End { get; set; }

    /// <summary>
    /// Исключить переводы, которые уже связаны с указанным заказом
    /// </summary>
    public int ExcludeOrderId { get; set; }
}