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
    /// Получить заказы за исключением тех, которые уже добавлены в данную доставку
    /// </summary>
    public int? ExcludeDeliveryId { get; set; }

    /// <inheritdoc/>
    public StatusesDocumentsEnum?[]? StatusesFilter { get; set; }

    /// <inheritdoc/>
    public DateTime? Start { get; set; }
    /// <inheritdoc/>
    public DateTime? End { get; set; }
}