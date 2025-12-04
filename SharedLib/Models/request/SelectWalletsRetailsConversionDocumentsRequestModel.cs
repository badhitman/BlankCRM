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
}