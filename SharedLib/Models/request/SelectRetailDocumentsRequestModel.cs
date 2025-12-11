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
    /// Ограничить вывод заказов только для указанного документа доставки
    /// </summary>
    public int? FilterDeliveryId { get; set; }

    /// <summary>
    /// Только заказы, которые не добавлены ни в одну доставку
    /// </summary>
    public bool WithoutDeliveriesOnly { get; set; }
}