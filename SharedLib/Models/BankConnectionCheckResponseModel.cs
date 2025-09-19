////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// BankConnectionCheckResponseModel
/// </summary>
public class BankConnectionCheckResponseModel : ResponseBaseModel
{
    /// <inheritdoc/>
    public List<BankTransferModelDB>? Operations { get; set; }
}