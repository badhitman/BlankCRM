////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectRetailDocumentsRequestModel
/// </summary>
public class SelectRetailDocumentsRequestModel
{
    /// <summary>
    /// BuyerFilterIdentityId
    /// </summary>
    public string[]? BuyersFilterIdentityId { get; set; }

    /// <summary>
    /// CreatorsFilterIdentityId
    /// </summary>
    public string[]? CreatorsFilterIdentityId { get; set; }

    /// <summary>
    /// Исключить по документу доставки
    /// </summary>
    public int? ExcludeDeliveryId { get; set; }
}