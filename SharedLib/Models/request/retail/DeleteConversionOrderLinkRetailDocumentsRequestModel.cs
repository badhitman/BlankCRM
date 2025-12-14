////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DeleteConversionOrderLinkRetailDocumentsRequestModel
/// </summary>
public class DeleteConversionOrderLinkRetailDocumentsRequestModel
{
    /// <inheritdoc/>
    public int ConversionId { get; set; }

    /// <inheritdoc/>
    public int OrderId { get; set; }

    /// <inheritdoc/>
    public int OrderConversionLinkId { get; set; }
}