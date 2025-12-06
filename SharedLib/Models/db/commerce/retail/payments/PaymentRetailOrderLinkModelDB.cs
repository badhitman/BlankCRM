////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// PaymentOrderLinkModelDB
/// </summary>
public class PaymentRetailOrderLinkModelDB : PaymentRetailLinkBaseModel
{
    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public RetailDocumentModelDB? Order { get; set; }
    
    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public int OrderId { get; set; }
}