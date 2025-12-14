////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// GetPaymentsRetailOrdersDocumentsRequestModel
/// </summary>
public class GetPaymentsRetailOrdersDocumentsRequestModel
{
    /// <summary>
    /// UserIdentityId
    /// </summary>
    public required int[] Ids { get; set; }
}