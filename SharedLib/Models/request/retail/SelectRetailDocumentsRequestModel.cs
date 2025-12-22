////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectRetailDocumentsRequestModel
/// </summary>
public class SelectRetailDocumentsRequestModel : SelectRetailDocumentsBaseModel
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
    /// Только заказы с рваными сумами
    /// </summary>
    public bool? EqualsSumFilter { get; set; }

    /// <summary>
    /// Получить заказы за исключением тех, которые уже добавлены в данную доставку
    /// </summary>
    public int? ExcludeDeliveryId { get; set; }
}