////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// RetailDeliveryOrderLinkModelDB
/// </summary>
public class RetailDeliveryOrderLinkModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public RetailDocumentModelDB? OrderDocument { get; set; }
    /// <inheritdoc/>
    public int OrderDocumentId { get; set; }


    /// <inheritdoc/>
    public DeliveryDocumentRetailModelDB? DeliveryDocument { get; set; }
    /// <inheritdoc/>
    public int DeliveryDocumentId { get; set; }
}