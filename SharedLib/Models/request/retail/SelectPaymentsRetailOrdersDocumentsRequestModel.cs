////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectPaymentsRetailOrdersDocumentsRequestModel
/// </summary>
public class SelectPaymentsRetailOrdersDocumentsRequestModel : SelectPaymentsRetailBaseRequestModel
{
    /// <summary>
    /// UserIdentityId
    /// </summary>
    public string? PayerFilterIdentityId { get; set; }

    /// <summary>
    /// Исключить из выдачи платежи, которые уже связаны с указанным заказом
    /// </summary>
    public int ExcludeOrderId { get; set; }
}