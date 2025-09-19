////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// TBankOperationsResponseModel
/// </summary>
public class TBankOperationsResponseModel
{
    /// <inheritdoc/>
    public List<TBankOperationModel>? Operations { get; set; }

    /// <inheritdoc/>
    public string? NextCursor { get; set; }
}